

using Core.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Infraesctructure.Interfaces;
using Infraesctructure.Services;
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

        public ICommand SearchCommand { get; private set; }

        public CallsInQueuesUCViewModel()
        {
            IsLoading = false;
            TotalRecords = 0;
            service = new CallsInQueuesProvider();

            SearchCommand = new RelayCommand(SearchAsync);

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

                    //var start = new Queue
                    //{
                    //    Name = "Todos Los Registors",
                    //    QueueID = "Todos Los Registros",
                    //    IsSelected = true,
                    //};
                    //filters.Insert(0,start);

                    QueuesFilter = new ObservableCollection<Queue>(filters);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                //  NotifiactionHelper
                //.SetMessage("Error", ex.Message.ToString(),
                //           NotificationType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;

                Thread.Sleep(5000);

                var list = await service.FetchAllAsync();



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
              //  NotifiactionHelper
              //.SetMessage("Error", ex.Message.ToString(),
              //           NotificationType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void SearchAsync()
        {

            var selectedQueues = QueuesFilter.Where(q => q.IsSelected).Select(q => q.Name).ToArray();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < selectedQueues.Length; i++)
            {
                sb.Append(selectedQueues[i]);

                if (i < selectedQueues.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            MessageBox.Show(sb.ToString());
        }
    }
}
