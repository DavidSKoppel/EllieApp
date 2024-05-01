using CommunityToolkit.Maui.Views;
using EllieApp.Models;

namespace EllieApp.CustomPopUp;

public partial class AlarmPopUp : Popup
{
	public AlarmPopUp(Alarm alarm)
	{
#if ANDROID
		InitializeComponent();
#endif
    }
}