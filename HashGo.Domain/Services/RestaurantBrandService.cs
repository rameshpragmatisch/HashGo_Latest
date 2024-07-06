using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Db;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Domain.ViewModels;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.HttpHelper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;

namespace HashGo.Domain.Services;

public class RestaurantBrandService : IRestaurantBrandService
{
    private const int DELAY_TIME_IN_MILLI_SEC = 100;

    private readonly ILoggingService _logger;
    private readonly Random randomNumberGenerator = new(DateTime.Now.Nanosecond);

    private IProductDetailStoreService _productService;
    private IReadOnlyCollection<ProductDetail> _products;
    private IReadOnlyCollection<TenantConnect> _tenantConnects;

    public RestaurantBrandService(ILoggingService loggingService, IProductDetailStoreService service)
    {
        _logger = loggingService;
        _productService = service;
    }

    public async Task<IReadOnlyCollection<RestaurantBrand>> ReadBrandsForConnect(
        IReadOnlyCollection<TenantConnect> connectItems)
    {
        _tenantConnects = connectItems;
        var brands = await this.ReadAllBrands();
        var selectedBrands = new List<RestaurantBrand>();

        foreach( var brand in brands)
        {
            var requiredBrand = connectItems.FirstOrDefault(x => x.TenantUniqueKey == brand.TenantUniqueKey);
            if(requiredBrand != null)
                selectedBrands.Add(brand);
        }

        return selectedBrands;
    }

    public async Task<IReadOnlyCollection<RestaurantBrand>> ReadAllBrands()
    {
        var restaurants = new List<RestaurantBrand>();
        _products = await _productService.ReadAllAsync();

        if (_products != null && _products.Any() && _tenantConnects != null && _tenantConnects.Any())
        {
            var activeTenants = _tenantConnects.Select(x => x.TenantUniqueKey);

            foreach (var item in _products)
            {
                // Check if tenant is active. Excluding old tenant data
                if (activeTenants.Contains(item.TenantUniqueKey))
                {
                    var deviceDetailsResponse = JsonConvert.DeserializeObject<BaseResponse<DineGoDeviceDetail>>(item.DeviceDetails);

                    if ((bool)deviceDetailsResponse?.Success)
                    {
                        foreach (var brand in deviceDetailsResponse.Result.DineGoBrands)
                        {
                            var requiredBrand = _tenantConnects.First(x => x.TenantUniqueKey == item.TenantUniqueKey);

                            var restaurant = new RestaurantBrand()
                            {
                                DeviceDetail = deviceDetailsResponse.Result,
                                Brand = brand,

                                Name = brand.Label,
                                Banner = brand.ThemeSettingObj?.BrandImage,
                                HomeLogo = brand.ThemeSettingObj?.HomeLogoImage,
                                BackgroundImage = brand.ThemeSettingObj?.StartImage,
                                IsActive = true,
                                DineGateWayId = GetDineGatewayId(brand.OrderLink),
                                DineGatewayToken = GetDineGatewayToken(brand.OrderLink),
                                WaitingTime = null,
                                TenantUniqueKey = item.TenantUniqueKey,
                                ScreenMenuId = brand.ScreenMenuId,
                                DepartmentNames = brand.DineGoDepartments is null ? [] : brand.DineGoDepartments.Select(x => x.DepartmentName).ToList(),
                            };

                            restaurants.Add(restaurant);
                        }
                    }
                }
            }
        }

        return restaurants;
    }

    public async Task<string> ReadBrandEstimateTimeAsync(RestaurantBrand Brand)
    {
        RestaurantBrand _selectedBrand = Brand;
        var _selectedBrandOrderLink= _selectedBrand.DeviceDetail.DineGoBrands.First().OrderLink;
        _selectedBrand.WaitingTime = GetDinePlanEstimatedWaitingTime(_selectedBrandOrderLink);
        return string.IsNullOrEmpty(_selectedBrand.WaitingTime) ? "N/A" : _selectedBrand.WaitingTime;
    }

    private string waiting_Time;
    private string GetDinePlanEstimatedWaitingTime(string orderLink)
    {
        if (!string.IsNullOrEmpty(orderLink))
        {
            var orderLinks = orderLink.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var IPAddressLink = orderLinks[1];
            string estimateWaitingTimeURL= IPAddressLink + ConnectApiRouterNames.ESTIMATE_WAITINGTIME;
            var myInstance = HttpHelper.GetInstance(estimateWaitingTimeURL);
            var responeString = myInstance.Get(estimateWaitingTimeURL);
            var data = JsonConvert.DeserializeObject<EstimateWaitingTimeResponseData>(responeString);
            if (data != null && data.Response != null && data.Response.Value != null)
                return waiting_Time = data.Response.Value;
            return null;
        }

        return null;
    }

    private string GetDineGatewayId(string orderLink)
    {
        if(!string.IsNullOrEmpty(orderLink))
        {
            var orderLinks = orderLink.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return orderLinks.Last();
        }

        return string.Empty;
    }

    private string GetDineGatewayToken(string orderLink)
    {
        if (!string.IsNullOrEmpty(orderLink))
        {
            var orderLinks = orderLink.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if(orderLinks != null && orderLinks.Count() > 1)
            {
                orderLinks.Remove(orderLinks.Last());
                return orderLinks.Last();
            }
        }

        return string.Empty;
    }

    public async Task<IReadOnlyCollection<ScreenMenu>> ReadMenuAsync(RestaurantBrand brand)
    {
        var menu = LoadData(brand);

        return menu;
    }

    public async Task<TimeSpan> ReadEstimatedWaitTimeAsync(long resturantId)
    {
        await Task.Delay(DELAY_TIME_IN_MILLI_SEC); // Artificial delay to give the impression of work

        var randomMinutes = randomNumberGenerator.Next(2, 25);

        return TimeSpan.FromMinutes(randomMinutes);
    }

    public async Task<WorkFlow> ReadWorkFlowAsync(MenuItem selectedMenu, string restaurantUniqueId, long menuItemId, DiningOption orderDiningOption,
        MealItOptions mealItOption)
    {
        await Task.Delay(DELAY_TIME_IN_MILLI_SEC); // Artificial delay to give the impression of work

        return GetWorkFlow_OrderTags(selectedMenu, 1, MealItOptionHelper.GetComboOptionName(mealItOption), "", restaurantUniqueId, menuItemId, orderDiningOption, mealItOption);
    }

    private IReadOnlyCollection<ScreenMenu> LoadData(RestaurantBrand restaurantBrand)
    {
        var screenMenu = new List<ScreenMenu>();
        var locationScreenMenu = new List<ScreenMenu>();
        try
        {
            if (_products.Any(x => x.TenantUniqueKey == restaurantBrand.TenantUniqueKey))
            {
                foreach (var product in _products.Where(x => x.TenantUniqueKey == restaurantBrand.TenantUniqueKey))
                {
                    var screenMenuResponse = JsonConvert.DeserializeObject<BaseResponse<ScreenMenuResponse>>(product.ScreenMenuDetails);
                    var locationProductResponse = JsonConvert.DeserializeObject<BaseResponse<ScreenCategoryResponse>>(product.LocationItemDetails);
                    var orderTagResponse = JsonConvert.DeserializeObject<BaseResponse<OrderTagResponse>>(product.OrderTagDetails);


                    bool isOrderTagsAvaiable = orderTagResponse != null && orderTagResponse.Success && orderTagResponse.Result != null && orderTagResponse.Result.Items.Any();
                    bool islocationProductsAvailable = locationProductResponse != null && locationProductResponse.Success && locationProductResponse.Result != null && locationProductResponse.Result.Categories.Any();

                    var orderTagsCache = new List<OrderTag>();
                    var locationMenuItemsCache = new List<MenuItem>();
                    var screenMenuItemsCache = new List<MenuItem>();
                    ScreenMenu? screenMenuObj = new ScreenMenu();

                    if (isOrderTagsAvaiable)
                        orderTagsCache = orderTagResponse.Result.Items.ToList();

                    if (islocationProductsAvailable)
                    {
                        var screenCatResponse = locationProductResponse.Result;
                        if (screenCatResponse != null)
                        {
                            screenCatResponse.Categories = screenCatResponse.Categories.OrderBy(c => c.SortOrder).ToArray();

                            foreach (var screenCategory in screenCatResponse.Categories)
                            {
                                foreach (var menu in screenCategory.MenuItems)
                                {
                                    var dd = JsonConvert.DeserializeObject<MenuPortion>(JsonConvert.SerializeObject(menu.NormalPortion));

                                    menu.CategorieId = screenCategory.Id;
                                    menu.MenuItemId = menu.Id;

                                    foreach (var myPortion in menu.MenuPortions)
                                        myPortion.MenuItemId = menu.Id;

                                    locationMenuItemsCache.Add(menu);
                                }
                            }
                        }
                    }

                    if (screenMenuResponse != null && screenMenuResponse.Success)
                    {
                        screenMenuObj = screenMenuResponse.Result.Menu.LastOrDefault(a => a.Id == restaurantBrand.ScreenMenuId);
                        if (screenMenuObj != null)
                        {
                            screenMenuObj.Categories = screenMenuObj.Categories.OrderBy(a => a.SortOrder).ToList();

                            foreach (var ca in screenMenuObj.Categories)
                            {
                                ca.MenuItems = ca.MenuItems.OrderBy(x => x.SortOrder).ToArray();
                                foreach (var mi in ca.MenuItems) screenMenuItemsCache.Add(mi);

                                if (!string.IsNullOrEmpty(ca.Files))
                                {
                                    var allImages = JsonConvert.DeserializeObject<List<ImageFile>>(ca.Files);
                                    if (allImages != null && allImages.Any(a => a.@default))
                                    {
                                        var myListImage = new List<ImageFile> { allImages.LastOrDefault(a => a.@default) };
                                        ca.HomeImagePath = JsonConvert.SerializeObject(myListImage);
                                    }
                                }
                            }
                        }
                    }

                    if (islocationProductsAvailable && screenMenuObj != null)
                    {
                        MapLocationMenuItems(screenMenuObj.Categories, locationMenuItemsCache);

                        foreach (var screenCategory in screenMenuObj.Categories)
                                foreach (var menu in screenCategory.MenuItems)
                                    if (isOrderTagsAvaiable)
                                        MapComboItem(menu, orderTagsCache, restaurantBrand.DepartmentNames, locationMenuItemsCache, screenMenuItemsCache);


                            locationScreenMenu.Add(new ScreenMenu() { Categories = screenMenuObj.Categories.ToList() });
                    }

                    if(screenMenuObj != null)
                        screenMenu.Add(screenMenuObj);
                }
            }

            if (locationScreenMenu.Any())
                MapLocationMenuToScreenMenu(screenMenu, locationScreenMenu.First().Categories);
        }
        catch (Exception ex)
        {
            this._logger.Trace($"{nameof(RestaurantBrandService)} : {nameof(LoadData)}() Error.");
        }

        return screenMenu;
    }


    private void MapLocationMenuItems(List<ScreenCategory> screenCategories, List<MenuItem> _locationMenuItemsCache)
    {
        foreach (var category in screenCategories)
        {
            foreach (var screenMenuItem in category.MenuItems)
            {
                var localMenuItem = _locationMenuItemsCache.FirstOrDefault(x => x.Id == screenMenuItem.MenuItemId);

                if (localMenuItem != null)
                {
                    screenMenuItem.MenuPortions = localMenuItem.MenuPortions;
                    screenMenuItem.CategorieId = localMenuItem.CategorieId;
                    screenMenuItem.RequiresAuth = localMenuItem.RequiresAuth;
                    screenMenuItem.MenuItemSchedules = localMenuItem.MenuItemSchedules;
                    screenMenuItem.Description = localMenuItem.Description;

                    if (localMenuItem.OrderTags != null)
                        screenMenuItem.OrderTags = localMenuItem.OrderTags;

                    if (localMenuItem.UpMenuItems != null)
                        screenMenuItem.UpMenuItems = new ObservableCollection<UpMenuItem>(localMenuItem.UpMenuItems);

                    if (localMenuItem.Combo != null)
                        screenMenuItem.Combo = localMenuItem.Combo;
                }
            }
        }
    }

    private void MapLocationMenuToScreenMenu(List<ScreenMenu> _screenMenu, List<ScreenCategory> _locationCategories)
    {
        foreach (var sMenu in _screenMenu)
        {
            foreach (var category in sMenu.Categories)
            {
                category.IsVisible = category.CheckSchedule();
                if (_locationCategories.Any(x => x.Name == category.Name))
                {
                    var locCategory = _locationCategories.Where(x => x.Name == category.Name).FirstOrDefault();

                    if (locCategory != null)
                    {
                        foreach (var item in category.MenuItems)
                        {
                            if (locCategory.MenuItems.Any(x => x.Name == item.Name))
                            {
                                var menuItem = locCategory.MenuItems.Where(x => x.Name == item.Name).First();
                                item.Description = menuItem.Description ?? menuItem.Description;
                                item.OrderTags = menuItem.OrderTags ?? item.OrderTags;
                                item.Combo = menuItem.Combo ?? menuItem.Combo;
                                item.MenuPortions = menuItem.MenuPortions ?? menuItem.MenuPortions;
                            }
                        }
                    }
                }

            }
        }
    }

    private void MapOrderTags(MenuItem menuItem, List<OrderTag> _orderTagsCache, List<string> departmentNames)
    {
        menuItem.OrderTags.Clear();
        bool departmentListExist = false;
        bool mappedDepartmentExist = false;

        if (menuItem != null && _orderTagsCache?.Count > 0)
        {
            foreach (var otItem in _orderTagsCache)
            {
                departmentListExist = otItem.DepartmentsList != null && otItem.DepartmentsList.Any();
                mappedDepartmentExist = departmentListExist && otItem.DepartmentsList.Any(x => departmentNames.Contains(x.DisplayText)) && departmentNames != null && departmentNames.Any();

                if ((departmentListExist && mappedDepartmentExist) || !departmentListExist)
                {
                    foreach (var nullMap in otItem.Maps.Where(x => x.CategoryId == null))
                        if (nullMap.MenuItemId == menuItem.MenuItemId.ToString())
                            menuItem.OrderTags.Add(
                                DeserializeObjectOrderTags(otItem));

                    foreach (var notNullMap in otItem.Maps.Where(x =>
                                 x.CategoryId != null && x.CategoryId == menuItem.CategorieId.ToString()))
                        if (string.IsNullOrEmpty(notNullMap.MenuItemId))
                        {
                            menuItem.OrderTags.Add(
                                DeserializeObjectOrderTags(otItem));
                        }
                        else
                        {
                            if (notNullMap.MenuItemId == menuItem.MenuItemId.ToString())
                                menuItem.OrderTags.Add(
                                    DeserializeObjectOrderTags(otItem));
                        }
                }

            }

            foreach (var orderTag in menuItem.OrderTags)
            {
                foreach (var item in orderTag.Tags)
                {
                    item.GroupName = orderTag.Name;
                    item.GroupMaxSelection = orderTag.MaxSelectedItems;
                }
            }
        }
    }

    private void MapComboItem(MenuItem menuItem, List<OrderTag> _orderTagsCache, List<string> departmentNames, List<MenuItem> lstMenuItemsFromLocation, List<MenuItem> lstScreenMenuItems)
    {
        MapOrderTags(menuItem, _orderTagsCache, departmentNames);

        var findLocalMenuItem = lstMenuItemsFromLocation.FirstOrDefault(x => x.Id == menuItem.MenuItemId);

        if (findLocalMenuItem?.Combo != null)
        {
            menuItem.Combo = JsonConvert.DeserializeObject<Combo>(JsonConvert.SerializeObject(findLocalMenuItem.Combo));
            SetDefaultCombo(menuItem);
            foreach (var cg in menuItem?.Combo?.ComboGroups)
                foreach (var ci in cg?.ComboItems)
                {
                    var ciLocation = lstMenuItemsFromLocation.FirstOrDefault(x => x.MenuItemId == ci.MenuItemId);
                    if (ciLocation != null)
                    {
                        ci.MenuItem = JsonConvert.DeserializeObject<MenuItem>(JsonConvert.SerializeObject(ciLocation));
                        if (!string.IsNullOrEmpty(ci.Files))
                        {
                            ci.MenuItem.Files = ci.Files;
                        }
                        else
                        {
                            var ciScreen = lstScreenMenuItems.FirstOrDefault(x => x.MenuItemId == ciLocation.Id);
                            if (ciScreen != null) ci.MenuItem.Files = ciScreen.Files;
                        }
                        MapComboItem(ci.MenuItem, _orderTagsCache, departmentNames, lstMenuItemsFromLocation, lstScreenMenuItems);
                    }
                }
        }
    }

    private void SetDefaultCombo(MenuItem menuItem)
    {
        if (menuItem?.Combo?.ComboGroups != null)
            foreach (var item in menuItem?.Combo?.ComboGroups)
                if (item.ComboItems != null)
                    if (item.Minimum > 0)
                    {
                        var defaultItem = item.ComboItems.FirstOrDefault();
                        if (defaultItem != null)
                        {
                            defaultItem.IsSelected = true;
                            defaultItem.ChooseQuantity = item.Minimum;
                            item.ComboQueue.Add(defaultItem);
                        }
                    }
    }

    private OrderTag DeserializeObjectOrderTags(OrderTag _OrderTag)
    {
        return JsonConvert.DeserializeObject<OrderTag>(JsonConvert.SerializeObject(_OrderTag));
    }

    private string LoadImageToBase64String(string image)
    {
        if (string.IsNullOrEmpty(image)) return string.Empty;

        try
        {
            var path = image;//string.Concat(Environment.CurrentDirectory, "\\", image);
            var imageArray = File.ReadAllBytes(path);
            var base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }
        catch (Exception ex)
        {
        }

        return string.Empty;
    }

    public WorkFlow GetWorkFlow_OrderTags(MenuItem selectedMenu, long workflowId, string workflowTitle, string workflowDescription, string restaurantUniqueId,
    long menuItemId, DiningOption orderDiningOption, MealItOptions mealItOption)
    {
        var workFlow = new WorkFlow
        {
            Id = workflowId,
            Name = workflowTitle,
            Description = workflowDescription,
            ResturantUniqueId = restaurantUniqueId,
            MenuItemId = menuItemId,
            Steps = new List<WorkFlowStep>()
        };

        if (selectedMenu.IsShowComboText)
        {
            // null check conditions are already in place inside the property IsShowComboText
            foreach (var comboGroup in selectedMenu.Combo.ComboGroups)
            {
                foreach (var comboItem in comboGroup.ComboItems)
                    if (comboItem.Name.ToUpper() == MealItOptionHelper.MealIt && mealItOption == MealItOptions.MealIt)
                        LoadOrderTagsByCombo(comboItem.MenuItem, ref workFlow);
            }
        }

        if (selectedMenu.OrderTags.Any())
            foreach (var _orderTag in selectedMenu.OrderTags)
                workFlow.Steps.Add(LoadOrderTag(_orderTag));

        return workFlow;
    }

    private void LoadOrderTagsByCombo(MenuItem selectedMenu, ref WorkFlow _Workflow )
    {
        WorkFlowStep workFlowStep = null;
        WorkFlowStepOption workFlowStepOption = null;

        foreach (var comboGroup in selectedMenu?.Combo?.ComboGroups)
        {
            workFlowStep = GetWorkFlowStep(1, comboGroup.Name, "", false, false);
            workFlowStep.Options = new List<WorkFlowStepOption>();
            workFlowStep.IsOptional = comboGroup.Minimum == 0;
            workFlowStep.SubHeader = GetSubHeader(workFlowStep.IsOptional, comboGroup.Minimum, comboGroup.Maximum);
            workFlowStep.MinimumQuantity = comboGroup.Minimum;
            workFlowStep.MaximumQuantity = comboGroup.Maximum;

            if (comboGroup.ComboItems.Any())
            {
                foreach (var tagItem in comboGroup.ComboItems)
                {
                    tagItem.MenuItem.GroupName = comboGroup.Name;
                    workFlowStepOption = GetWorkFlowStepOption(1, workFlowStep.Id, tagItem.MenuItem, comboGroup);
                    workFlowStep.Options.Add(workFlowStepOption);
                }
            }
            _Workflow.Steps.Add(workFlowStep);
        }

       
    }

    private string GetSubHeader(bool isOptional, int Min, int Max)
    {
        var subHeader = new StringBuilder();

        if (isOptional)
            subHeader.Append("Optional,");

        if (Min > 0)
            subHeader.Append("Choose ");

        if (Min > 0)
            subHeader.Append(Min.ToString());

        if (Min > 0 && Max > 1 )
            subHeader.Append(", ");

        if ((!isOptional && Max > 1) || (isOptional && Max > 0))
            subHeader.Append(" Max " + Max.ToString());

        return subHeader.ToString();
    }

    private WorkFlowStep LoadOrderTag(OrderTag orderTagItem)
    {
        WorkFlowStep workFlowStep = null;
        WorkFlowStepOption workFlowStepOption = null;

        workFlowStep = GetWorkFlowStep(1, orderTagItem.Name, "", false, false);
        workFlowStep.Options = new List<WorkFlowStepOption>();
        workFlowStep.IsOptional = orderTagItem.MinSelectedItems == 0;
        workFlowStep.SubHeader = GetSubHeader(workFlowStep.IsOptional, orderTagItem.MinSelectedItems, orderTagItem.MaxSelectedItems);
        workFlowStep.MinimumQuantity = orderTagItem.MinSelectedItems;
        workFlowStep.MaximumQuantity = orderTagItem.MaxSelectedItems;

        if (orderTagItem.Tags.Any())
        {
            foreach (var tagItem in orderTagItem.Tags.OrderBy(x=>x.SortOrder))
            {
                workFlowStepOption = GetWorkFlowStepOption(1, workFlowStep.Id, tagItem);
                workFlowStep.Options.Add(workFlowStepOption);
            }
        }

        return workFlowStep;
    }

    public WorkFlowStep GetWorkFlowStep(long workFlowStepId, string workflowStepTitle, string workflowStepDescription,
        bool isOptional, bool isMultiSelectValue)
    {
        return new WorkFlowStep
        {
            Id = workFlowStepId,
            Description = workflowStepDescription,
            Name = workflowStepTitle,
            Status = WorkflowStepStatus.None,
            IsOptional = isOptional,
            IsMultiSelectValue = isMultiSelectValue
        };
    }

    public WorkFlowStepOption GetWorkFlowStepOption(long workFlowStepOptionId, long workFlowStepId, Tag tagItem)
    {
        tagItem.TotalQuantity = 0;

        var stepOption = new WorkFlowStepOption
        {
            Id = workFlowStepOptionId,
            Title = tagItem.Name,
            Description = string.Empty,
            Amount = AppSettings.CurrencySymbol != null ? AppSettings.CurrencySymbol : "$ " + tagItem.Price.ToString(),
            Icon = tagItem.Files,
            WorkFlowStepId = workFlowStepId,
            //CanAddQuantity = true,
            Quantity = tagItem.ChooseQuantity,
            OrderTagItem = tagItem,
            TotalQuantity = tagItem.TotalQuantity,
            MenuItem = new MenuItem()
        };

        return stepOption;
    }

    public WorkFlowStepOption GetWorkFlowStepOption(long workFlowStepOptionId, long workFlowStepId, MenuItem menuItem, ComboGroup combogroup)
    {
        menuItem.TotalQuantity = 0;

        var stepOption = new WorkFlowStepOption
        {
            Id = workFlowStepOptionId,
            Title = menuItem.DisplayName,
            Description = string.Empty,
            Amount = AppSettings.CurrencySymbol != null ? AppSettings.CurrencySymbol : "$ " + menuItem.NormalPortion.Price.ToString(),
            Icon = menuItem.Files,
            WorkFlowStepId = workFlowStepId,
            //CanAddQuantity = true,
            Quantity = menuItem.ChooseQuantity,
            MenuItem = menuItem,
            TotalQuantity = menuItem.TotalQuantity,
            OrderTagItem = new Tag()
        };

        return stepOption;
    }

    public WorkFlowStepOption GetWorkFlowStepOption(long workFlowStepOptionId, string workFlowStepOptionTitle,
        string workFlowStepOptionDescription, double amount, string imagePath, long workFlowStepId,
        bool canAddQuantity = false, IEnumerable<string> tagValue = null)
    {
        var stepOption = new WorkFlowStepOption
        {
            Id = workFlowStepOptionId,
            Title = workFlowStepOptionTitle,
            Description = workFlowStepOptionDescription,
            //Amount = amount,
            Icon = imagePath,
            WorkFlowStepId = workFlowStepId,
            CanAddQuantity = canAddQuantity,
            Quantity = 0
        };

        ////if (!string.IsNullOrEmpty(imagePath))
        ////    stepOption.Icon = LoadImageToBase64String("\\Resources\\Images\\" + imagePath);

        //if (tagValue?.Any() == true) stepOption.Tags = new List<string>(tagValue);

        return stepOption;
    }
}