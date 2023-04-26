namespace FinalProject.Controllers
{
    public class UndoRedoController
    {
        public Stack<DocumentModel> UndoStack;
        public Stack<DocumentModel> RedoStack;
        public DocumentModel Current;

        public UndoRedoController(DocumentModel doc)
        {
            UndoStack = new Stack<DocumentModel>();
            RedoStack = new Stack<DocumentModel>();
            Current = doc;
        }

        public void Add(DocumentModel model)
        {
            UndoStack.Push(Current);
            Current = model;
            RedoStack.Clear();
        }

        public DocumentModel? Undo()
        {
            if (UndoStack.Count == 0)
                return null;

            RedoStack.Push(Current);
            Current = UndoStack.Pop();
            return Current.Clone();
        }

        public DocumentModel? Redo()
        {
            if (RedoStack.Count == 0)
                return null;

            UndoStack.Push(Current);
            Current = RedoStack.Pop();
            return Current.Clone();
        }

        public void reset(DocumentModel current)
        {
            Current = current;
            UndoStack.Clear();
            RedoStack.Clear();
        }
    }
}
