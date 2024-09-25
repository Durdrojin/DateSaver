﻿namespace DateSaver;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editDateId;
    private DateTime currentDate = DateTime.Now;
    private int countDown;

    public MainPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        dbService.UpdateCountDown();

        Task.Run(async () => listView.ItemsSource = await _dbService.GetDates());
    }

    private async void newBtn_Clicked(object sender, EventArgs e)
    {

        // Go to create page
        //await Shell.Current.GoToAsync("CreatePage");
        await Navigation.PushModalAsync(new CreatePage(_dbService, _editDateId));

        listView.ItemsSource = await _dbService.GetDates();
    }

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var date = (Date)e.Item;
        countDown = (date.DateSaved.Date - currentDate.Date).Days;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":

                await Navigation.PushModalAsync(new CreatePage(_dbService, _editDateId, e));

                break;
            case "Delete":

                await _dbService.Delete(date);
                listView.ItemsSource = await _dbService.GetDates();

                break;
        }
    }
}
