using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HashGo.Core.Models;

public class RestaurantBrand : NameIdBase, INotifyPropertyChanged
{
    public string Description { get { return this.Brand.Label; }  }

    public string? AccentColor { get { return this.BrandThemeSetting.PrimaryButtonColor; } }

    public string? HomeBackgroundColor { get { return this.BrandThemeSetting.StartupBrandBgColor; } }

    public string Logo { get { return DeviceDetail.ThemeSettingsObj.Logo; } }

    public string HomeLogo { get; set; }

    public string BackgroundImage { get; set; }

    public string Banner { get; set; }
    public List<string> DepartmentNames { get; set; }

    public bool IsActive { get; set; }
    public int ScreenMenuId { get; set; }

    public string TenantUniqueKey { get; set; }

    public string DineGateWayId { get; set; }

    public string DineGatewayToken { get; set; }

    public int DepartmentCount { get; set; }

    private string waitingTime;
    public string WaitingTime
    {
        get { return waitingTime; }
        set { waitingTime = value;
            OnPropertyChanged(nameof(WaitingTime));
        }
    }


    public DineGoBrandThemeSetting BrandThemeSetting { get { return Brand.ThemeSettingObj; } }

    public DineGoBrand Brand { get; set; }

    public DineGoDeviceDetail DeviceDetail { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}