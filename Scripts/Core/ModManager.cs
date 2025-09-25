using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

public class ModManager : MonoBehaviour
{
    public static ModManager Instance { get; private set; }

    // --- Базы данных ---
    public Dictionary<string, ItemDefinition> AllItems { get; private set; } = new Dictionary<string, ItemDefinition>();
    public Dictionary<string, NeedDefinition> AllNeeds { get; private set; } = new Dictionary<string, NeedDefinition>();
    public Dictionary<string, TraitDefinition> AllTraits { get; private set; } = new Dictionary<string, TraitDefinition>();
    public Dictionary<string, RecipeDefinition> AllRecipes { get; private set; } = new Dictionary<string, RecipeDefinition>();
    public Dictionary<string, BodyPartDefinition> AllBodyParts { get; private set; } = new Dictionary<string, BodyPartDefinition>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAllContent();
    }

    private void LoadAllContent()
    {
        Debug.Log("--- Загрузка контента началась ---");
        string coreDataPath = Path.Combine(Application.streamingAssetsPath, "Core");
        string modsRootPath = Path.Combine(Application.dataPath, "..", "Mods");

        // Сначала загружаем основной контент игры
        LoadModContent(coreDataPath);

        // Затем загружаем пользовательские моды
        if (!Directory.Exists(modsRootPath)) Directory.CreateDirectory(modsRootPath);
        foreach (string modDirectory in Directory.GetDirectories(modsRootPath))
        {
            LoadModContent(modDirectory);
        }

        Debug.Log($"--- Загрузка завершена ---");
        Debug.Log($"Предметов:{AllItems.Count}, Рецептов:{AllRecipes.Count}, Потребностей:{AllNeeds.Count}, Черт:{AllTraits.Count}, Частей тела:{AllBodyParts.Count}");
    }

    private void LoadModContent(string modPath)
    {
        if (!Directory.Exists(modPath)) return;
        Debug.Log($"Загрузка из: {modPath}");

        // --- ЗАГРУЗКА ДАННЫХ ИЗ ФАЙЛОВ-СПИСКОВ ---
        LoadDefinitions<ItemDefinition>(modPath, "Items.json", AllItems, def => def.id, "предмет");
        LoadDefinitions<RecipeDefinition>(modPath, "Recipes.json", AllRecipes, def => def.id, "рецепт");
        LoadDefinitions<NeedDefinition>(modPath, "NeedTypes.json", AllNeeds, def => def.id, "потребность");
        LoadDefinitions<TraitDefinition>(modPath, "TraitTypes.json", AllTraits, def => def.id, "черту");
        LoadDefinitions<BodyPartDefinition>(modPath, "BodyParts.json", AllBodyParts, def => def.id, "часть тела");

        // --- ЗАГРУЗКА КОДА (DLL) ---
        string codePath = Path.Combine(modPath, "Assemblies");
        if (Directory.Exists(codePath))
        {
            foreach (string filePath in Directory.GetFiles(codePath, "*.dll"))
            {
                Assembly modAssembly = Assembly.LoadFrom(filePath);
                foreach (var type in modAssembly.GetTypes())
                {
                    if (typeof(IModInitializer).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IModInitializer initializer = (IModInitializer)System.Activator.CreateInstance(type);
                        initializer.Initialize();
                        Debug.Log($"Инициализирован код мода из сборки: {Path.GetFileName(filePath)}");
                    }
                }
            }
        }
    }

    private void LoadDefinitions<T>(string modPath, string fileName, Dictionary<string, T> database, System.Func<T, string> getId, string logName)
    {
        // Все определения лежат в папке Definitions
        string filePath = Path.Combine(modPath, "Definitions", fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<T> definitions = JsonConvert.DeserializeObject<List<T>>(json);

            if (definitions == null)
            {
                Debug.LogWarning($"Не удалось прочитать данные из файла {fileName} в {modPath}. Файл пуст или имеет неверный формат.");
                return;
            }

            foreach (var def in definitions)
            {
                string id = getId(def);
                if (id != null)
                {
                    // Позволяет модам перезаписывать ванильные определения
                    database[id] = def;
                }
            }
            Debug.Log($"Загружено/обновлено {definitions.Count} {logName}(ов) из {fileName}");
        }
    }
}

// Интерфейс, который должны реализовывать все моды для загрузки кода
public interface IModInitializer
{
    void Initialize();
}

