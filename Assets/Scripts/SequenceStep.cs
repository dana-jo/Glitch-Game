[System.Serializable]
public struct SequenceStep
{
    public BlockType type;
    public int repeatCount;

    public SequenceStep(BlockType type, int repeatCount = 0)
    {
        this.type = type;
        this.repeatCount = repeatCount;
    }
}