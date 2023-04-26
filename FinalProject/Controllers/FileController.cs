namespace FinalProject.Controllers
{
    public class FileController
    {
        public string CurrentFilename = "";
        public bool Modified { get; set; } = false;

        public OpenFileDialog OpenFileDialog { get; } = new();
        public SaveFileDialog SaveFileDialog { get; } = new();
        public Action<string> OnSave { get; }
        public Action<string> OnOpen { get; }

        public FileController(Action<string> onSave, Action<string> onOpen)
        {
            OpenFileDialog.Filter = $"JSON files (*.json))|*.json|All files (*.*)|*.*";
            SaveFileDialog.Filter = $"JSON files (*.json))|*.json|All files (*.*)|*.*";
            this.OnSave = onSave;
            this.OnOpen = onOpen;
        }

        public static bool YesNo(string msg)
        {
            return MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo)
                   == DialogResult.Yes;
        }

        public bool CheckModifiedAndCanContinue()
        {
            if (!Modified)
                return true;
            if (!YesNo("Document modified, do you want to save?"))
                return true;
            return SaveCurrentFile();
        }

        public bool SaveAs()
        {
            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
                return false;
            CurrentFilename = SaveFileDialog.FileName;
            OnSave(CurrentFilename);
            Modified = false;
            return true;
        }

        public bool SaveCurrentFile()
        {
            if (string.IsNullOrEmpty(CurrentFilename))
                return SaveAs();

            OnSave(CurrentFilename);
            Modified = false;
            return true;
        }

        public bool Open()
        {
            if (!CheckModifiedAndCanContinue())
                return false;

            if (OpenFileDialog.ShowDialog() != DialogResult.OK)
                return false;

            CurrentFilename = OpenFileDialog.FileName;
            OnOpen(CurrentFilename);
            Modified = false;
            return true;

        }

        public bool NewWithCheck()
        {
            if (!CheckModifiedAndCanContinue())
                return false;
            Modified = false;
            CurrentFilename = "";
            return true;
        }
    }
}
