using Prism.Mvvm;

namespace LabCarDeskTop.ViewModels;

public class LabCarDTViewModel : BindableBase
{
  private string _title = "Control LabCar";
  public string Title
  {
    get { return _title; }
    set { SetProperty(ref _title, value); }
  }

  private string _LabelCarLab = "";
  public string LabelCarLab
  {
    get { return _LabelCarLab; }
    set { SetProperty(ref _title, value); }
  }

  
  public LabCarDTViewModel()
  {
    _LabelCarLab = "Выбор теста ";
  }
}
