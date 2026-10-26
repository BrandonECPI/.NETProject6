using System.Collections.ObjectModel;
using MAUI_Data_Access.DataAccess;
using MAUI_Data_Access.Models;

namespace MAUI_Data_Access
{
	public partial class MainPage : ContentPage
	{
		// Data access layer for the People table
		private readonly PersonData personData;

		// List shown in the CollectionView; updates the screen when it changes
		public ObservableCollection<Person> People { get; set; } = new();

		public MainPage()
		{
			InitializeComponent();
			personData = new PersonData();
			BindingContext = this;
			_ = UpdatePeopleList();
		}

		// Checks the input, saves the person, then refreshes the list
		private async void OnSaveClicked(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtFirstName.Text))
			{
				await DisplayAlert("Error", "First name cannot be empty", "OK");
				return;
			}

			if (string.IsNullOrWhiteSpace(txtLastName.Text))
			{
				await DisplayAlert("Error", "Last name cannot be empty", "OK");
				return;
			}

			if (dpDateOfBirth.Date is not DateTime dob)
			{
				await DisplayAlert("Error", "Please pick a date of birth", "OK");
				return;
			}

			if (dob.Date > DateTime.Today)
			{
				await DisplayAlert("Error", "Date of Birth cannot be in the future", "OK");
				return;
			}

			var person = new Person
			{
				FirstName = txtFirstName.Text.Trim(),
				LastName = txtLastName.Text.Trim(),
				DoB = dob.Date
			};

			await personData.SavePersonAsync(person);

			// Clear the form for the next entry
			txtFirstName.Text = string.Empty;
			txtLastName.Text = string.Empty;

			await UpdatePeopleList();
		}

		// Reloads the People list from the database
		private async Task UpdatePeopleList()
		{
			List<Person> people = await personData.GetPeopleAsync();
			People.Clear();
			foreach (Person p in people)
			{
				People.Add(p);
			}
		}
	}
}