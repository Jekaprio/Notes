using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows;


public static class LanguageManager
{
    private static ResourceManager _resourceManager;

    public static void Initialize()
    {
        // Инициализируем ресурсный менеджер для файла ресурсов по умолчанию (например, "Resources.en-US.resx").
        _resourceManager = new ResourceManager("Resources.Resources", typeof(LanguageManager).Assembly);


        // Установим начальный язык
        SetLanguage("uk-UA");
    }

    public static void SetLanguage(string cultureCode)
    {
        // Создаем новую культуру на основе выбранного кода
        CultureInfo newCulture = new CultureInfo(cultureCode);

        // Устанавливаем культуру приложения
        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;

        // Путь к XAML-ресурсам
        string xamlResourcePath = $"Resources/Resources.{cultureCode}.xaml";

        // Создаем новый ResourceDictionary и загружаем XAML-ресурсы
        ResourceDictionary dict = new ResourceDictionary();
        try
        {
            dict.Source = new Uri(xamlResourcePath, UriKind.Relative);
        }
        catch (Exception)
        {
            // Обработка ошибки, если файл XAML-ресурсов не найден
            // Может быть заменена на логирование или другую обработку ошибок
        }

        // Очищаем существующие ресурсы и добавляем новый ResourceDictionary
        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(dict);
    }


    public static string GetString(string key)
    {
        return _resourceManager.GetString(key);
    }
}
