using CURSR.Utils;

namespace CURSR.Saves
{
    [System.Serializable]
    public abstract class SaveData
    {
        protected abstract string FileName { get; }
        
        private IOFile file => new IOFile(FileName);
        
        public virtual void Save()
        {
            file.Save(this);
        }

        public SaveData Load()
        {
            return file.Load<SaveData>();
        }
    }
}