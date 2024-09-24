using FarmWinform.Dtos;
using FarmWinform.Services;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmWinform
{
    public partial class Form1 : Form
    {
        private readonly IAnimalService _animalService;
        private AnimalDTO animal = new AnimalDTO();

        // Constructor accepting IAnimalService for dependency injection
        public Form1(IAnimalService animalService)
        {
            _animalService = animalService;
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
            await LoadAnimalTypesAsync();
        }

        private async Task LoadDataAsync()
        {
            dataGridView.AutoGenerateColumns = false;
            var animalData = await _animalService.GetAllAnimalsAsync();
            dataGridView.DataSource = animalData;
        }

        private async Task LoadAnimalTypesAsync()
        {
            var animalTypes = await _animalService.GetAllAnimalTypesAsync();
            cbAnimalType.DataSource = animalTypes;
            cbAnimalType.DisplayMember = "AnimalName";
            cbAnimalType.ValueMember = "AnimalTypeId";
        }

        private async void btSave_Click(object sender, EventArgs e)
        {
            if (!ValidateField()) return;

            animal.AnimalTypeId = (int)cbAnimalType.SelectedValue;
            animal.MilkProduced = double.Parse(tbMilk.Text.Trim());
            animal.OffspringCount = int.Parse(tbOffspring.Text.Trim());

            await _animalService.SaveAnimalAsync(animal);
            Clear();
            await LoadDataAsync();
            MessageBox.Show("Submitted Successfully!");
        }

        private bool ValidateField()
        {
            if (cbAnimalType.Text.Trim() == "")
            {
                MessageBox.Show("Choose the Animal Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbAnimalType.Focus();
                return false;
            }

            if (tbMilk.Text.Trim() == "")
            {
                MessageBox.Show("Enter the amount of milk", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbMilk.Focus();
                return false;
            }

            if (tbOffspring.Text.Trim() == "")
            {
                MessageBox.Show("Enter the number of offspring", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbOffspring.Focus();
                return false;
            }
            return true;
        }

        private async void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow.Index == -1) return;

            animal.AnimalId = Convert.ToInt32(dataGridView.CurrentRow.Cells["dgAnimalID"].Value);
            var selectedAnimal = (await _animalService.GetAllAnimalsAsync())
                .FirstOrDefault(a => a.AnimalId == animal.AnimalId);

            if (selectedAnimal != null)
            {
                cbAnimalType.SelectedValue = selectedAnimal.AnimalTypeId;
                tbMilk.Text = selectedAnimal.MilkProduced.ToString();
                tbOffspring.Text = selectedAnimal.OffspringCount.ToString();

                btSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            await _animalService.DeleteAnimalAsync(animal.AnimalId);
            await LoadDataAsync();
            Clear();
            MessageBox.Show("Deleted Successfully!");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            cbAnimalType.SelectedIndex = -1;
            tbMilk.Clear();
            tbOffspring.Clear();
            btnDelete.Enabled = false;
        }

        private void tbMilk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void tbOffspring_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private async void btnGetSound_Click(object sender, EventArgs e)
        {
            string sounds = await _animalService.GetFarmSoundsAsync();
            MessageBox.Show($"The sounds heard in the farm: {sounds}");
        }

        private async void btnGenerateReport_Click(object sender, EventArgs e)
        {
            var updatedAnimals = await _animalService.GetAllAnimalsAfterBreedingAndMilkingAsync();
            var report = new StringBuilder();

            foreach (var animal in updatedAnimals)
            {
                report.AppendLine($"Animal ID: {animal.AnimalId}, Type: {animal.AnimalTypeName}, Offspring Born: {animal.OffspringGenerated}, Milk Produced: {animal.MilkProducedInRound} liters");
            }

            MessageBox.Show(report.ToString(), "Farm Report");
            await LoadDataAsync();
        }
    }

}
