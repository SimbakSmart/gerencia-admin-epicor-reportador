

using Core.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Infraesctructure.Interfaces;
using Infraesctructure.Services;
using Notifications.Wpf;
using Presentation.Helpers;
using Presentation.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModels.UC
{
    public class CallsInQueuesUCViewModel:ViewModelBase
    {
        ICallsInQueuesProvider service;

        private ObservableCollection<CallsInQueues> itemList;

        public ObservableCollection<CallsInQueues> ItemList
        {
            get { return itemList; }
            set
            {
                itemList = value;
                RaisePropertyChanged(nameof(ItemList));
            }
        }
        private ObservableCollection<Queue> queuesFilter;

        public ObservableCollection<Queue> QueuesFilter
        {
            get { return queuesFilter; }
            set
            {
                queuesFilter = value;
                RaisePropertyChanged(nameof(QueuesFilter));
            }
        }


        private bool isLoading;

        public bool IsLoading
        {
            get { return isLoading; }

            set
            {
                isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }




        private int totalRecords;

        public int TotalRecords
        {
            get { return totalRecords; }
            set
            {
                totalRecords = value;
                RaisePropertyChanged(nameof(TotalRecords));
            }
        }


        private string searchByNumber;

        public string SearchByNumber
        {
            get { return searchByNumber; }

            set
            {
                searchByNumber = value;
                RaisePropertyChanged(nameof(SearchByNumber));
            }
        }


        public ICommand SearchCommand { get; private set; }
    
        public RelayCommand<KeyEventArgs> SearchByNumberKeyDownCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public CallsInQueuesUCViewModel()
        {
            IsLoading = false;
            TotalRecords = 0; 
            service = new CallsInQueuesProvider();

            SearchCommand = new AsyncRelayCommand(SearchAsync);

            SearchByNumberKeyDownCommand = new RelayCommand<KeyEventArgs>(SearchByNumberKeyDown);
            RefreshCommand = new AsyncRelayCommand(RefrehAsync);

            Task.Run(async () =>
            {
                await LoadFilters();
                await LoadDataAsync();

            });

        }

     


        private async Task LoadFilters()
        {
            try
            {
                IsLoading = true;

                var filters = await service.FetchQueuesAsync();
                
                if (filters != null)
                {
                    QueuesFilter = new ObservableCollection<Queue>(filters);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                NotifiactionMessage
                    .SetMessage("Error", GlobalMessages.INTERNAL_SERVER_ERROR, NotificationType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadDataAsync(string queryParams="")
        {
            try
            {
                IsLoading = true;

               // Thread.Sleep(5000);

                var list = await service.FetchAllAsync(queryParams);

                if (list != null) 
                {
                   TotalRecords = list.Count;
                    ItemList = new ObservableCollection<CallsInQueues>(list);
                }

               var filters = await service.FetchQueuesAsync();
                if(filters != null)
                {
                    QueuesFilter = new ObservableCollection<Queue>(filters);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                NotifiactionMessage
                    .SetMessage("Error", GlobalMessages.INTERNAL_SERVER_ERROR, NotificationType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SearchAsync()
        {

            try
            {
                var selectedQueues = QueuesFilter.Where(q => q.IsSelected).Select(q => q.Name).ToArray();

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < selectedQueues.Length; i++)
                {
                    sb.Append($"'{selectedQueues[i]}'");

                    if (i < selectedQueues.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }

                if (string.IsNullOrEmpty(sb.ToString()) ||
                    string.IsNullOrWhiteSpace(sb.ToString()))
                {
                    NotifiactionMessage
                    .SetMessage("No Valido", "Es necesario ingresar un valor de busqueda",
                               NotificationType.Error);
                    return;
                }

                string queryParam = $" AND  Que.Name IN ({sb})";

                await LoadDataAsync(queryParam);
                ClearFilters();
                NotifiactionMessage
               .SetMessage("Información", GlobalMessages.SUCCESS,
                       NotificationType.Success);

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message.ToString());
                NotifiactionMessage
                    .SetMessage("Error", GlobalMessages.INTERNAL_SERVER_ERROR, NotificationType.Error);
            }
            
        }

        private async void SearchByNumberKeyDown(KeyEventArgs args)
        {
            try
            {     
                if (args.Key == Key.Enter)
                {

                    if (string.IsNullOrEmpty(SearchByNumber) ||
                     string.IsNullOrWhiteSpace(SearchByNumber))
                    {
                        NotifiactionMessage
                        .SetMessage("No Valido", "Es necesario ingresar un valor de busqueda",
                                   NotificationType.Error);
                        return;
                    }

                    string number = SearchByNumber;
                    //string queryParam = $" AND Sc.Number={number} OR Av.ValueShortText={number}"; //OR VALUE= {number}
                    string queryParam = $" AND Sc.Number='{number}'  OR Av.ValueShortText='{number}' "; //OR VALUE= {number}
                    await LoadDataAsync(queryParam);
                    NotifiactionMessage
                   .SetMessage("Información", GlobalMessages.SUCCESS,
                           NotificationType.Success);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                NotifiactionMessage
                    .SetMessage("Error", GlobalMessages.INTERNAL_SERVER_ERROR, NotificationType.Error);
            }
        }



        public async Task RefrehAsync()
        {
            try
            {
                IsLoading = false;
                await LoadDataAsync(string.Empty);
                ClearFilters();
                NotifiactionMessage
               .SetMessage("Información", GlobalMessages.REFRESH,
                       NotificationType.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                NotifiactionMessage
                    .SetMessage("Error", GlobalMessages.INTERNAL_SERVER_ERROR, NotificationType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearFilters()
        {
            SearchByNumber = string.Empty;
        }


    }
}
