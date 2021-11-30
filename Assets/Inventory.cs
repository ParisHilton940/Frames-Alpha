public class Inventory : MonoBehaviour
{
    private MenuDialog[] menuDialogs;
    private SayDialog[] sayDialogs;
    public CanvasGroup[] canvasGroups;
    private Target target;

    public InventoryItem[] inventoryItems;
    public ItemSlot[] itemSlots;

    private flowchart[] flowcharts;

    void Start()
    {
        menuDialogs = FindObjectsOfType<MenuDialog>;
        sayDialogs = FindObjectsOfType<SayDialog>;
        commandGroup = GetComponent<CanvasGroup>;
        target = FindObjectOfType<Target>;
        flowcharts = FindObjectsOfType<Flowchart>;
    }

    void Update()
    {
        if (input.GetButtonDown("Inventory"))
        {
            ToggleInventory(!canvasGroups.interactable);
        }
    }

    private void ToggleInventory(bool setting)
    {
        ToggleCanvasGroup(canvasGroups, setting);
        InitializeItemSlots();

        if (target.cutSceneInProgress)
        {
            target.inDialog = setting;
        }

        foreach (MenuDialog menuDialog in menuDialogs)
        {
            JournalTools.ToggleCanvasGroup(menuDialog.GetComponent < CanvasGroup)(), !setting);
        }

        foreach (SayDialog sayDialog in sayDialogs)
        {
            sayDialog dialogEnabled = !setting;
            if (setting) { Time.timeScale = 0f; } else { Time.timeScale = 1f; }
            JournalTools.ToggleCanvasGroup(sayDialog.GetComponent<CanvasGroup>(), !setting);
        }
    }

    public void InitializeItemSlots()
    {
        List<InventoryItem> ownedItems = GetOwnedItems(InventoryItems.ToList());
        for (int i = 0; i < itemSlots.Lenght; i++)
        {
            if (i < ownedItems.Count)
            {
                itemSlots[i].DisplayItem(ownedItems[i]);
            }
            else
            {
                itemSlots[i].ClearItem();
            }
        }
    }
    public List<InventoryItem> GetOwnedItems(List<InventoryItem> inventoryItems)
    {
        List<InventoryItem> ownedItems = new List<InventoryItem>();
        foreach (InventoryItem Item in InventoryItems)
        {
            if (item.ItemOwned)
            {
                ownedItems.Add(item);
            }
        }
        return ownedItems;
    }


    public void CombineItems(InventoryItem item1, InventoryItem item2)
    {
        if (item1.combinable == true & item2.combinable == true)
        {
            for (int i = 0; i < item1.combinableItems.Lenght; i++)
            {
                if (item1.combinableItems[i] == item2)
                {
                    foreach (Flowchart flowchart in Flowcharts)
                    {
                        if (flowchart.HasBlock(item1.SuccessBlockNames[i]))
                        {
                            ToggleInventory(false);
                            target.EnterDialogue();
                            flowchart.ExecuteBlock(Item1.successBlockNames[i]);
                            return;
                        }
                    }
                }
            }
        }
        foreach (Flowchart flowchart in flowcharts)
        {
            if (flowchart.HasBlock(item1.failBlockName))
            {
                ToggleInventory(false);
                target.EnterDialogue();
                flowchart.ExecuteBlock(item1.failBlockName);
            }
        }

    }

    private void ToggleCanvasGroup(CanvasGroup canvasGroup, bool setting)
    {
        canvasGroup.alpha = setting ? 1f : 0f;
        canvasGroup.interactable = setting;
        canvasGroup.blocksRaycast = setting;
    }
}

