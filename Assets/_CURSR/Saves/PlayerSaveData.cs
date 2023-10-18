using CURSR.Saves;

namespace CURSR.Saves
{
    [System.Serializable]
    public class PlayerSaveData : SaveData
    {
        protected override string FileName => "playerData.save";
        public int Currency;
        public string Seed;
        public float[] PlayerPos;
    }
}