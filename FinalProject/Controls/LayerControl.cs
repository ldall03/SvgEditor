namespace FinalProject.Controls
{
    public partial class LayerControl : UserControl
    {
        public bool IsVisible { get; set; } = true;
        public bool IsLocked { get; set; } = false;
        public string LayerName { get; set; }
        public LayerModel LayerModel { get; set; }

        // Events
        public event EventHandler onLayerSelected;
        public event EventHandler onToggleVisible;
        public event EventHandler onToggleLocked;
        public event EventHandler onSendUp;
        public event EventHandler onSendDown;
        public event EventHandler onRemove;

        public LayerControl(string name)
        {
            InitializeComponent();
            LayerName = name;
            textBox1.Text = name;
            LayerModel = new LayerModel(name);

            visible_Button.BackColor = Color.Gray;
        }

        public void form_Click(object sender, EventArgs e)
        {
            onLayerSelected.Invoke(this, e);
        }

        public void visible_Clicked(object sender, EventArgs e)
        {
            IsVisible = !IsVisible;
            LayerModel.IsVisible = IsVisible;
            onToggleVisible.Invoke(this, e);
            visible_Button.BackColor = (IsVisible) ? Color.Gray : Color.White;
        }

        public void locked_Clicked(object sender, EventArgs e)
        {

            IsLocked = !IsLocked;
            LayerModel.IsLocked = IsLocked;
            locked_Button.BackColor = (IsLocked) ? Color.Gray : Color.White;
        }

        public void sendUp_Clicked(object sender, EventArgs e)
        {
            onSendUp.Invoke(this, e);
        }

        public void sendDown_Clicked(object sender, EventArgs e)
        {
            onSendDown.Invoke(this, e);
        }

        public void remove_Clicked(object sender, EventArgs e)
        {
            onRemove.Invoke(this, e);
        }

        private void nameChanged(object sender, EventArgs e)
        {
            LayerName = textBox1.Text;
            LayerModel.LayerName = LayerName;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
