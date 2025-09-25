// Scripts/Gameplay/Jobs/JobDriver_Haul.cs
using UnityEngine;

public class JobDriver_Haul : JobDriver
{
    private enum HaulStep { GoToItem, PickupItem, GoToStockpile, DropItem }
    private HaulStep _currentStep = HaulStep.GoToItem;
    public override string JobType => JobDefOf.Haul;

    public override void Tick()
    {
        if (Job.TargetA == null)
        {
            Debug.LogError($"JobDriver_Haul не может работать: TargetA is null! Завершаю задачу.");
            OnFinish();
            return;
        }

        switch (_currentStep)
        {
            case HaulStep.GoToItem:
                Pawn.Position = Job.TargetA.Position;
                _currentStep = HaulStep.PickupItem;
                break;

            case HaulStep.PickupItem:
                var itemOnGround = (Item)Job.TargetA;

                // --- НОВАЯ ЛОГИКА ---
                // Пытаемся положить предмет в инвентарь.
                if (Pawn.Inventory.TryAddItemToInventory(itemOnGround))
                {
                    WorldController.Instance.CurrentMap.RemoveEntity(itemOnGround);
                    Debug.Log($"{Pawn.Name} взял {itemOnGround.Def.name} в инвентарь.");
                    _currentStep = HaulStep.GoToStockpile;
                }
                else
                {
                    Debug.LogWarning($"{Pawn.Name}: Недостаточно места в инвентаре для {itemOnGround.Def.name}!");
                    OnFinish(); // Завершаем работу, если не можем поднять
                }
                break;

            case HaulStep.GoToStockpile:
                Pawn.Position = Job.TargetB;
                _currentStep = HaulStep.DropItem;
                break;

            case HaulStep.DropItem:
                // --- НОВАЯ ЛОГИКА ---
                // Выбрасываем все из инвентаря на землю
                var droppedItems = Pawn.Inventory.DropAllInventory();
                foreach (var item in droppedItems)
                {
                    WorldController.Instance.CurrentMap.SpawnEntity(item, Pawn.Position);
                }
                Debug.Log($"{Pawn.Name} разгрузил инвентарь на складе.");
                OnFinish();
                break;
        }
    }
}
