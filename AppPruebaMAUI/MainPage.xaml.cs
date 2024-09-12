
namespace AppPruebaMAUI
{
    public partial class MainPage : ContentPage
    {
        private List<Producto> _productos;
        private Stream _selectedImageStream;
        private string _selectedImageFilePath;

        public MainPage()
        {
            InitializeComponent();
            _productos = new List<Producto>();
            ProductsListView.ItemsSource = _productos;

        }

        private void OnAddProductButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text) ||
           !decimal.TryParse(ProductPriceEntry.Text, out var price) ||
           string.IsNullOrEmpty(_selectedImageFilePath))
            {
                DisplayAlert("Error", "Por favor ingresa todos los campos correctamente.", "OK");
                return;
            }

            var producto = new Producto
            {
                Name = ProductNameEntry.Text,
                Price = price,
                ImageFilePath = _selectedImageFilePath
            };

            _productos.Add(producto);
            ProductsListView.ItemsSource = null; // Restablecer la fuente de datos
            ProductsListView.ItemsSource = _productos;

            // Limpiar los campos
            ProductNameEntry.Text = string.Empty;
            ProductPriceEntry.Text = string.Empty;
            ProductImage.Source = null;
            _selectedImageFilePath = null;
        }

        private async void OnSelectImageButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var fileResult = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecciona una imagen",
                    FileTypes = FilePickerFileType.Images
                });

                if (fileResult != null)
                {
                    var fileName = Path.GetFileName(fileResult.FullPath);
                    var localFilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                    using (var sourceStream = await fileResult.OpenReadAsync())
                    using (var localFileStream = File.OpenWrite(localFilePath))
                    {
                        await sourceStream.CopyToAsync(localFileStream);
                    }

                    // Almacenar la ruta del archivo en una variable para uso posterior
                    _selectedImageFilePath = localFilePath;
                    ProductImage.Source = ImageSource.FromFile(localFilePath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo seleccionar la imagen: {ex.Message}", "OK");
            }
        }
    }
    

}
