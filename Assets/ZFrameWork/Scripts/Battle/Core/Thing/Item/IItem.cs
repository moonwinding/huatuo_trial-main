[System.Serializable]
public class Item: Thing
{
    public float Value;
    public ItemType ItemType;

    public override bool OnUse(int pUseCount) {
        if (Count < pUseCount)
            return false;
        Count -= pUseCount;
        OnUseSuccess(pUseCount);
        return true;
    }
    public virtual void OnUseSuccess(int pUseCount) { }
}

