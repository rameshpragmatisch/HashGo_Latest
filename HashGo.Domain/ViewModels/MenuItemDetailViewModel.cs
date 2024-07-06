using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Models;
using HashGo.Domain.Models;
using System.Collections.ObjectModel;
using HashGo.Core.Enum;
using HashGo.Infrastructure;

namespace HashGo.Domain.ViewModels
{
    public partial class MenuItemDetailViewModel : BaseWorkFlowViewModel
    {
        private ICartService cartService;
        

        [ObservableProperty]
        public WorkFlow workFlow;

        [ObservableProperty]
        private WorkFlowStep selectedWorkFlowStep;

        [ObservableProperty]
        private IEnumerable<WorkFlowStepOptionModel> workFlowStepOptionModels;

        [ObservableProperty]
        private bool showAddToCart = false;

        [ObservableProperty]
        private bool isEditOrder = false;


        [ObservableProperty]
        private TagWithQuantity[] editOrderTagsWithQuantity;

        public MenuItemDetailViewModel(ILoggingService loggingService,
            IRestaurantBrandService resturantBrandService,
                                       INavigationService navigationService,
                                       IOrderService orderService,
                                       ICartService cartService)
            : base(loggingService, resturantBrandService, navigationService, orderService)
        {
            this.cartService = cartService;
        }

        partial void OnSelectedWorkFlowStepChanged(WorkFlowStep value)
        {
            if (value != null && value.Options != null)
            {
                var models = new ObservableCollection<WorkFlowStepOptionModel>();

                foreach (var item in value.Options)
                {
                    models.Add(new WorkFlowStepOptionModel(item));
                }

                this.WorkFlowStepOptionModels = models;
            }
            else
            {
                this.WorkFlowStepOptionModels = Enumerable.Empty<WorkFlowStepOptionModel>();
            }
        }

        protected override async Task InitializeDataAsync()
        {
            this.WorkFlow = await this.ResturantBrandService.ReadWorkFlowAsync(this.SelectedMenuItem, this.SelectedRestaurant.TenantUniqueKey, this.SelectedMenuItem.Id, Core.Enum.DiningOption.TakeAway, this.SelectedMealItOption);

            if (IsEditOrder)
            {
                RePopulateEditOrderItems();
            }

            if (this.WorkFlow != null && this.WorkFlow.Steps?.Any() == true)
            {
                SelectedWorkFlowStep = this.WorkFlow.Steps.First();
                SelectedWorkFlowStep.IsActiveSelection = true;
            }
            else
            {
                await AddItemToCart();
            }
        }


        protected override async Task LoadDataAsync()
        {
            //this.OrderService.SetOrderDiningOption
        }

        private bool CanSelectWorkFlowOptionValue(WorkFlowStepOption stepOption) { return !this.IsLoading; }


        [RelayCommand(CanExecute = nameof(CanSelectWorkFlowOptionValue))]
        private async Task SelectWorkFlowOptionValue(WorkFlowStepOption stepOption)
        {
            if (this.WorkFlowStepOptionModels?.Any(x => x.IsSelected) == true)
            {
                foreach (var model in this.WorkFlowStepOptionModels.Where(y => y.IsSelected))
                {
                    model.IsSelected = false;
                }

                //this.WorkFlowStepOptionModels.Toi.ForEach(x => x.IsSelected = false);
            }

            this.SelectedWorkFlowStep.Value = stepOption;
            this.MoveToNextStepCommand.NotifyCanExecuteChanged();

            var option = this.WorkFlowStepOptionModels?.FirstOrDefault(x => x.Data.Id == stepOption.Id);
            if (option != null)
            {
                option.IsSelected = true;
            }

            AddQuantityForSingleItem(stepOption);
        }


        private bool CanMoveToNextStep(WorkFlowStep step)
        {
            if (!this.IsLoading && step != null)
            {
                IsQualifiedForAddToCart(step);

                if (step.IsOptional || step.IsMinimumQuantityChosen)
                    return true;
                else
                    return false;
            }

            return false;
        }

        private void IsQualifiedForAddToCart(WorkFlowStep step)
        {
            var index = this.WorkFlow.Steps.IndexOf(step);

            if (index == this.WorkFlow.Steps.Count - 1)
                ShowAddToCart = AllWorkFlowStepQuantityChosen();
            else
                ShowAddToCart = false;
        }


        [RelayCommand(CanExecute = nameof(CanMoveToNextStep))]
        private async Task MoveToNextStep(WorkFlowStep step)
        {
            if (step != null)
            {
                var index = this.WorkFlow.Steps.IndexOf(step);
                if (index < this.WorkFlow.Steps.Count - 1)
                {
                    this.SelectedWorkFlowStep = this.WorkFlow.Steps[index + 1];
                    ResetSelections(SelectedWorkFlowStep);

                    var newIndex = this.WorkFlow.Steps.IndexOf(this.SelectedWorkFlowStep);

                    if (newIndex == this.WorkFlow.Steps.Count - 1)
                        this.AddItemToCartCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private void ResetSelections(WorkFlowStep step)
        {
            if (WorkFlow.Steps != null && WorkFlow.Steps.Any() && step != null)
            {
                foreach (var item in this.WorkFlow.Steps)
                    if(item.SelectedTagsWithQuantities==null || item.SelectedTagsWithQuantities.Count==0)
                        item.IsActiveSelection = false;

                this.WorkFlow.Steps.First(x => x.Name == step.Name).IsActiveSelection = true;
            }
        }

        private bool CanMoveToPreviousStep(WorkFlowStep step)
        {
            if (!this.IsLoading && step != null)
            {
                if (this.WorkFlow.Steps.IndexOf(step) != 0)
                {
                    return true;
                }
            }

            return false;
        }


        [RelayCommand(CanExecute = nameof(CanMoveToPreviousStep))]
        private async Task MoveToPreviousStep(WorkFlowStep step)
        {
            if (step != null)
            {
                var index = this.WorkFlow.Steps.IndexOf(step);
                if (index > 0)
                {
                    this.SelectedWorkFlowStep = this.WorkFlow.Steps[index - 1];
                    ResetSelections(SelectedWorkFlowStep);
                    this.AddItemToCartCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private bool AllWorkFlowStepQuantityChosen()
        {
            return WorkFlow.Steps.All(x => x.IsOptional || x.IsMinimumQuantityChosen);
        }


        private bool CanAddItemToCart()
        {
            return !this.IsLoading;
        }

        [RelayCommand(CanExecute = nameof(CanAddItemToCart))]

        private async Task AddItemToCart()
        {
            this.Logger.Trace($"{nameof(MenuItemDetailViewModel)} : {nameof(AddItemToCart)}() Started.");


            try
            {
                var cartItem = await this.cartService.AddOrderTagsToCart(this.OrderId, this.SelectedMenuItem, this.SelectedMealItOption, 1, WorkFlow.SelectedTagsForAllGroups);
                if (cartItem != null)
                {
                    cartItem.ResturantUniqueId = SelectedRestaurant.TenantUniqueKey;
                    cartItem.WaitingTime = SelectedRestaurant.WaitingTime;
                    cartItem.RestaurantBanner = SelectedRestaurant.Banner;
                    cartItem.BrandColor = SelectedRestaurant.BrandThemeSetting.BrandColor;
                    cartItem.WorkFlowName = WorkFlow.Name;

                    var parameters = new Dictionary<string, object>
                {
                    { nameof(this.OrderId), this.OrderId },
                    { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
                    { nameof(this.SelectedMenuItem), this.SelectedMenuItem },
                    { "Amount", cartItem.Price },
                    { "PageToNavigate", Pages.RestaurantHome },
                };

                    await NavigationService.NavigateToAsync(Pages.ItemAddedToCart.ToString(), parameters);
                }
                else
                {
                    this.Logger.Info("Item was not added to the cart");
                }
            }
            catch (Exception ex)
            {
                this.Logger.TraceException(ex);
            }
            finally
            {
                this.Logger.Trace($"{nameof(MenuItemDetailViewModel)} : {nameof(AddItemToCart)}() Completed.");
            }
        }

        private void InitializeWorkFlowObjects()
        {
            if (SelectedWorkFlowStep.SelectedTagsWithQuantities is null)
                SelectedWorkFlowStep.SelectedTagsWithQuantities = new ObservableCollection<TagWithQuantity>();

            if (WorkFlow.SelectedTagsForAllGroups is null)
                WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>();
        }

        [RelayCommand]
        private void AddQuantityForSingleItem(WorkFlowStepOption sOpt)
        {
            InitializeWorkFlowObjects();

            if (sOpt.OrderTagItem != null && sOpt.OrderTagItem.Id > 0)
            {
                if (sOpt.OrderTagItem.MaxQuantity <= 1)
                {
                    if (!SelectedWorkFlowStep.SelectedTagsWithQuantities.Any(x => x.OrderTagItem.Name == sOpt.OrderTagItem.Name))
                    {
                        sOpt.TotalQuantity += 1;
                        sOpt.OrderTagItem.TotalQuantity = sOpt.TotalQuantity;

                        SelectedWorkFlowStep.SelectedTagsWithQuantities.Add(new TagWithQuantity()
                        {
                            OrderTagItem = sOpt.OrderTagItem,
                            MenuItem = new MenuItem(),
                            Quantity = sOpt.TotalQuantity,
                            DisplayValue = "+ 1" + " x " + sOpt.OrderTagItem.Name + " " + GetDisplayAmount(sOpt.OrderTagItem, 1),
                            TotalPrice = GetTotalPrice(sOpt.OrderTagItem),
                            TotalQuantity = sOpt.TotalQuantity,
                            GroupDisplayName = sOpt.OrderTagItem.GroupName,
                            WorkFlowName = WorkFlow.Name,
                            ResturantUniqueId = GetRestaurantUniqueId()
                        });
                    }

                    if (SelectedWorkFlowStep.SelectedTagsWithQuantities.Sum(x => x.TotalQuantity) > sOpt.OrderTagItem.MaxQuantity)
                    {
                        ResetQuantitySelectionforOtherItems(sOpt.OrderTagItem.GroupName, sOpt.OrderTagItem.Name);

                        while (SelectedWorkFlowStep.SelectedTagsWithQuantities.Count() > sOpt.OrderTagItem.GroupMaxSelection)
                        {
                            SelectedWorkFlowStep.SelectedTagsWithQuantities.RemoveAt(0);
                        }
                    }

                    if (WorkFlow.SelectedTagsForAllGroups is null) WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>();

                    if (WorkFlow.SelectedTagsForAllGroups.Any(x => x.OrderTagItem?.GroupName == sOpt.OrderTagItem?.GroupName))
                        WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>(WorkFlow.SelectedTagsForAllGroups.Where(x => x.OrderTagItem?.GroupName != sOpt.OrderTagItem?.GroupName));

                    foreach (var item in SelectedWorkFlowStep.SelectedTagsWithQuantities)
                        WorkFlow.SelectedTagsForAllGroups.Add(item);

                    SelectedWorkFlowStep.RaiseSelectedTagsWithQuantities();
                }
                else
                {
                    AddQuantity(sOpt);
                }
            }
            else
            {
                if (sOpt.MenuItem != null && sOpt.MenuItem.Id > 0)
                {
                    if (!SelectedWorkFlowStep.SelectedTagsWithQuantities.Any(x => x.MenuItem?.Name == sOpt.MenuItem?.Name))
                    {
                        sOpt.TotalQuantity += 1;
                        sOpt.MenuItem.TotalQuantity = sOpt.TotalQuantity;

                        ResetQuantitySelectionforOtherItems(sOpt.MenuItem.GroupName, sOpt.MenuItem.Name);

                        SelectedWorkFlowStep.SelectedTagsWithQuantities.Add(new TagWithQuantity()
                        {
                            MenuItem = sOpt.MenuItem,
                            Quantity = sOpt.TotalQuantity,
                            DisplayValue = "+ 1" + " x " + sOpt.MenuItem.Name + " " + GetDisplayAmount(sOpt.MenuItem, 1),
                            TotalPrice = GetTotalPrice(sOpt.MenuItem),
                            TotalQuantity = sOpt.TotalQuantity,
                            OrderTagItem = new Tag(),
                            GroupDisplayName = sOpt.MenuItem.GroupName,
                            WorkFlowName = WorkFlow.Name,
                            ResturantUniqueId = GetRestaurantUniqueId()
                        });
                    }

                    while (SelectedWorkFlowStep.SelectedTagsWithQuantities.Count() > sOpt.MenuItem.ChooseQuantity)
                    {
                        SelectedWorkFlowStep.SelectedTagsWithQuantities.RemoveAt(0);
                    }

                    if (WorkFlow.SelectedTagsForAllGroups.Any(x => x.MenuItem?.GroupName == sOpt.MenuItem?.GroupName))
                        WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>(WorkFlow.SelectedTagsForAllGroups.Where(x => x.MenuItem?.GroupName != sOpt.MenuItem?.GroupName));

                    foreach (var item in SelectedWorkFlowStep.SelectedTagsWithQuantities)
                        WorkFlow.SelectedTagsForAllGroups.Add(item);

                    SelectedWorkFlowStep.RaiseSelectedTagsWithQuantities();
                }
            }

            CheckForItemWithZeroQuantity();
            CheckForMinimumQuantityChosen(sOpt);

            sOpt.CanAddQuantity = true;

        }

        private void ResetQuantitySelectionforOtherItems(string groupName, string menuItemName)
        {
            foreach (var step in workFlow.Steps)
            {
                if (step.Name == groupName)
                {
                    foreach (var item in step.Options)
                        if (item.Title.ToUpper() != menuItemName.ToUpper())
                            item.TotalQuantity = 0;
                }
            }
        }

        private void UpdateQuantityofOtherItem(string groupName, string menuItemName, int quantity)
        {
            foreach (var step in workFlow.Steps)
            {
                if (step.Name == groupName)
                {
                    foreach (var item in step.Options)
                        if (item.Title == menuItemName)
                            item.TotalQuantity = quantity;
                }
            }
        }

        private void CheckForItemWithZeroQuantity()
        {
            if (workFlow?.SelectedTagsForAllGroups != null && workFlow.SelectedTagsForAllGroups.Any())
                workFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>(WorkFlow.SelectedTagsForAllGroups.Where(x => x.TotalQuantity > 0));
        }

        [RelayCommand]
        private void AddQuantity(WorkFlowStepOption sOpt)
        {
            if (sOpt.OrderTagItem.Id > 0 && sOpt.TotalQuantity < sOpt.OrderTagItem.MaxQuantity)
            {
                if (SelectedWorkFlowStep.SelectedTagsWithQuantities is null) SelectedWorkFlowStep.SelectedTagsWithQuantities = new ObservableCollection<TagWithQuantity>();

                sOpt.TotalQuantity += 1;
                sOpt.OrderTagItem.TotalQuantity = sOpt.TotalQuantity;

                //if (SelectedWorkFlowStep.SelectedTagsWithQuantities.Sum(x => x.TotalQuantity) > sOpt.OrderTagItem.MaxQuantity)
                //    ResetQuantitySelectionforOtherItems(sOpt.OrderTagItem.GroupName, sOpt.OrderTagItem.Name);

                if (sOpt.TotalQuantity == sOpt.OrderTagItem.MaxQuantity)
                {
                    foreach (var item in SelectedWorkFlowStep.Options)
                        if (item.Title != sOpt.Title)
                            item.TotalQuantity = 0;

                    SelectedWorkFlowStep.SelectedTagsWithQuantities.Clear();
                }

                if (SelectedWorkFlowStep.SelectedTagsWithQuantities.Any(x => x.OrderTagItem?.Name == sOpt.OrderTagItem?.Name))
                    SelectedWorkFlowStep.SelectedTagsWithQuantities.Remove(SelectedWorkFlowStep.SelectedTagsWithQuantities.First(x => x.OrderTagItem?.Name == sOpt.OrderTagItem?.Name));

                SelectedWorkFlowStep.SelectedTagsWithQuantities.Add(new TagWithQuantity()
                {
                    OrderTagItem = sOpt.OrderTagItem,
                    TotalQuantity = sOpt.TotalQuantity,
                    DisplayValue = "+ " + sOpt.TotalQuantity + " x " + sOpt.OrderTagItem.Name + " " + GetDisplayAmount(sOpt.OrderTagItem),
                    TotalPrice = GetTotalPrice(sOpt.OrderTagItem),
                    MenuItem = new MenuItem(),
                    GroupDisplayName = sOpt.OrderTagItem.GroupName,
                    WorkFlowName = WorkFlow.Name,
                    ResturantUniqueId = GetRestaurantUniqueId()
                });


                var _totalQuan = SelectedWorkFlowStep.SelectedTagsWithQuantities.Sum(x => x.TotalQuantity);


                // Remove one quantity from those quantity that are greater than 1
                while (_totalQuan > sOpt.OrderTagItem.MaxQuantity && SelectedWorkFlowStep.SelectedTagsWithQuantities.Where(x => x.TotalQuantity > 1).Count() > 0)
                {
                    foreach (var item in SelectedWorkFlowStep.SelectedTagsWithQuantities)
                    {
                        if (item.TotalQuantity > 0)
                        {
                            item.TotalQuantity -= 1;
                            UpdateQuantityofOtherItem(item.OrderTagItem.GroupName, item.OrderTagItem.Name, item.TotalQuantity);
                            _totalQuan = SelectedWorkFlowStep.SelectedTagsWithQuantities.Sum(x => x.TotalQuantity);

                            if (_totalQuan == sOpt.OrderTagItem.MaxQuantity)
                                break;
                        }
                    }
                }

                // Remove order tags if count of selected orders exceeds max limit quantity
                while (_totalQuan > sOpt.OrderTagItem.MaxQuantity)
                {
                    foreach (var item in SelectedWorkFlowStep.SelectedTagsWithQuantities)
                    {
                        if (item.TotalQuantity > 0)
                        {
                            item.TotalQuantity -= 1;
                            UpdateQuantityofOtherItem(item.OrderTagItem.GroupName, item.OrderTagItem.Name, item.TotalQuantity);
                            _totalQuan = SelectedWorkFlowStep.SelectedTagsWithQuantities.Sum(x => x.TotalQuantity);

                            if (_totalQuan == sOpt.OrderTagItem.MaxQuantity)
                                break;
                        }
                    }
                }

                SelectedWorkFlowStep.SelectedTagsWithQuantities = new ObservableCollection<TagWithQuantity>(SelectedWorkFlowStep.SelectedTagsWithQuantities.Where(x => x.TotalQuantity > 0));

                RecalculateTotalPrice();

                if (WorkFlow.SelectedTagsForAllGroups is null) WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>();

                if (WorkFlow.SelectedTagsForAllGroups.Any(x => x.GroupName == sOpt.OrderTagItem?.GroupName))
                    WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>(WorkFlow.SelectedTagsForAllGroups.Where(x => x.OrderTagItem?.GroupName != sOpt.OrderTagItem?.GroupName));

                foreach (var item in SelectedWorkFlowStep.SelectedTagsWithQuantities)
                    WorkFlow.SelectedTagsForAllGroups.Add(item);


                sOpt.RaiseSelectedTagsWithQuantities();

                SelectedWorkFlowStep.RaiseSelectedTagsWithQuantities();
            }
            else
            {
                if (sOpt.MenuItem.Id > 0)
                    AddQuantityForSingleItem(sOpt);
            }

            CheckForItemWithZeroQuantity();
            CheckForMinimumQuantityChosen(sOpt);

            IsQualifiedForAddToCart(SelectedWorkFlowStep);

        }

        private void CheckForMinimumQuantityChosen(WorkFlowStepOption sOpt)
        {
            if (SelectedWorkFlowStep.SelectedTagsWithQuantities != null)
            {
                if (SelectedWorkFlowStep.SelectedTagsWithQuantities.Sum(x => x.TotalQuantity) >= SelectedWorkFlowStep.MinimumQuantity)
                    SelectedWorkFlowStep.IsMinimumQuantityChosen = true;
                else
                    SelectedWorkFlowStep.IsMinimumQuantityChosen = false;

            }
            this.SelectedWorkFlowStep.Value = sOpt;
            this.MoveToNextStepCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void SubtractQuantity(WorkFlowStepOption sOpt)
        {
            if (sOpt.TotalQuantity > 0)
            {
                if (SelectedWorkFlowStep.SelectedTagsWithQuantities is null) SelectedWorkFlowStep.SelectedTagsWithQuantities = new ObservableCollection<TagWithQuantity>();

                sOpt.TotalQuantity -= 1;
                sOpt.OrderTagItem.TotalQuantity = sOpt.TotalQuantity;

                if (SelectedWorkFlowStep.SelectedTagsWithQuantities.Any(x => x.OrderTagItem.Id > 0 && x.OrderTagItem.Name == sOpt.OrderTagItem.Name))
                    SelectedWorkFlowStep.SelectedTagsWithQuantities.Remove(SelectedWorkFlowStep.SelectedTagsWithQuantities.First(x => x.OrderTagItem.Name == sOpt.OrderTagItem?.Name));

                if (SelectedWorkFlowStep.SelectedTagsWithQuantities.Any(x => x.MenuItem.Id > 0 && x.MenuItem.Name == sOpt.MenuItem.Name))
                    SelectedWorkFlowStep.SelectedTagsWithQuantities.Remove(SelectedWorkFlowStep.SelectedTagsWithQuantities.First(x => x.MenuItem.Name == sOpt.MenuItem.Name));

                if (sOpt.TotalQuantity > 0)
                {
                    SelectedWorkFlowStep.SelectedTagsWithQuantities.Add(new TagWithQuantity()
                    {
                        OrderTagItem = sOpt.OrderTagItem,
                        MenuItem = new MenuItem(),
                        TotalQuantity = sOpt.TotalQuantity,
                        DisplayValue = "+ " + sOpt.TotalQuantity + " x " + sOpt.OrderTagItem.Name + " " + GetDisplayAmount(sOpt.OrderTagItem),
                        TotalPrice = GetTotalPrice(sOpt.OrderTagItem),
                        GroupDisplayName = sOpt.OrderTagItem.GroupName,
                        WorkFlowName = WorkFlow.Name,
                        ResturantUniqueId = GetRestaurantUniqueId()
                    });
                }

                RecalculateTotalPrice();

                if (sOpt.OrderTagItem.Id > 0 && WorkFlow.SelectedTagsForAllGroups.Any(x => x.OrderTagItem.GroupName == sOpt.OrderTagItem.GroupName))
                    WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>(WorkFlow.SelectedTagsForAllGroups.Where(x => x.OrderTagItem.GroupName != sOpt.OrderTagItem.GroupName));

                if (sOpt.MenuItem.Id > 0 && WorkFlow.SelectedTagsForAllGroups.Any(x => x.MenuItem.GroupName == sOpt.MenuItem.GroupName))
                    WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>(WorkFlow.SelectedTagsForAllGroups.Where(x => x.MenuItem.GroupName != sOpt.MenuItem.GroupName));

                foreach (var item in SelectedWorkFlowStep.SelectedTagsWithQuantities)
                    WorkFlow.SelectedTagsForAllGroups.Add(item);

            }

            CheckForItemWithZeroQuantity();
            CheckForMinimumQuantityChosen(sOpt);
        }

        private string GetRestaurantUniqueId()
        {
            return this.SelectedRestaurant.TenantUniqueKey;
        }

        private void RecalculateTotalPrice()
        {
            foreach (var item in SelectedWorkFlowStep.SelectedTagsWithQuantities)
            {
                item.TotalPrice = GetTotalPrice(item);

                if (item.OrderTagItem != null)
                    item.DisplayValue = "+ " + item.TotalQuantity + " x " + item.OrderTagItem.Name + " " + GetDisplayAmount(item.OrderTagItem);
            }
        }

        private string GetCurrencyFormat()
        {
            return AppSettings.CurrencySymbol != null ? AppSettings.CurrencySymbol : "$ ";
        }

        private decimal GetTotalPrice(Tag orderTagItem, int quantity = 0)
        {
            if (quantity > 0)
                return decimal.Round(orderTagItem.Price * quantity, AppSettings.Decimals, MidpointRounding.AwayFromZero);
            else
                return decimal.Round(orderTagItem.Price * orderTagItem.TotalQuantity, AppSettings.Decimals, MidpointRounding.AwayFromZero);
        }

        private decimal GetTotalPrice(TagWithQuantity tagWithQuantity, int quantity = 0)
        {
            var totalPrice = 0.0M;

            if (tagWithQuantity.OrderTagItem != null && tagWithQuantity.OrderTagItem.Price > 0)
            {
                if (quantity > 0)
                    totalPrice = totalPrice = decimal.Round(tagWithQuantity.OrderTagItem.Price * quantity, AppSettings.Decimals, MidpointRounding.AwayFromZero);
                else
                    totalPrice = decimal.Round(tagWithQuantity.OrderTagItem.Price * tagWithQuantity.TotalQuantity, AppSettings.Decimals, MidpointRounding.AwayFromZero);
            }

            return totalPrice;
        }

        private string GetDisplayAmount(Tag orderTagItem, int quantity = 0)
        {
            if (orderTagItem.Price > 0)
                return GetCurrencyFormat() + GetTotalPrice(orderTagItem, quantity);
            else
                return string.Empty;

        }

        private decimal GetTotalPrice(MenuItem menuItem, int quantity = 0)
        {
            if (menuItem.NormalPortion != null)
            {
                if (quantity > 0)
                    return decimal.Round(menuItem.NormalPortion.Price * quantity, AppSettings.Decimals, MidpointRounding.AwayFromZero);
                else
                    return decimal.Round(menuItem.NormalPortion.Price * menuItem.TotalQuantity, AppSettings.Decimals, MidpointRounding.AwayFromZero);
            }
            else
                return 0;
        }

        private string GetDisplayAmount(MenuItem menuItem, int quantity = 0)
        {
            if (menuItem.NormalPortion != null && menuItem.NormalPortion.Price > 0)
                return GetCurrencyFormat() + GetTotalPrice(menuItem, quantity);
            else
                return string.Empty;

        }

        private void RePopulateEditOrderItems()
        {
            var menuItems = EditOrderTagsWithQuantity.Select(x => x.MenuItem);
            var orderTagItems = EditOrderTagsWithQuantity.Select(x => x.OrderTagItem);

            foreach (var step in WorkFlow.Steps)
            {
                step.SelectedTagsWithQuantities = new ObservableCollection<TagWithQuantity>(EditOrderTagsWithQuantity.Where(x => x.GroupName == step.Name));
                step.IsMinimumQuantityChosen = true;

                foreach (var stepOption in step.Options)
                {
                    if (menuItems.Any(x => x.GroupName == step.Name && x.Name == stepOption.Title))
                    {
                        stepOption.MenuItem = menuItems.First(x => x.GroupName == step.Name && x.Name == stepOption.Title);
                        stepOption.CanAddQuantity = true;
                        stepOption.TotalQuantity = EditOrderTagsWithQuantity.First(x => x.GroupName == step.Name).TotalQuantity;
                    }

                    if (orderTagItems.Any(x => x.GroupName == step.Name && x.Name == stepOption.Title))
                    {
                        stepOption.OrderTagItem = orderTagItems.First(x => x.GroupName == step.Name && x.Name == stepOption.Title);
                        stepOption.CanAddQuantity = true;
                        stepOption.TotalQuantity = EditOrderTagsWithQuantity.First(x => x.GroupName == step.Name && stepOption.OrderTagItem.Name == x.OrderTagItem.Name).TotalQuantity;
                    }
                }
            }

            WorkFlow.SelectedTagsForAllGroups = new ObservableCollection<TagWithQuantity>(EditOrderTagsWithQuantity);
        }

        [RelayCommand]
        private async Task SelectedItemChanged(WorkFlowStep step)
        {
            IsQualifiedForAddToCart(step);
            ResetSelections(step);
        }
    }
}
