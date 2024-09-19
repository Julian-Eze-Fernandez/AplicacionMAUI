using System.Collections.ObjectModel;

namespace AppPruebaMAUI;

public partial class ContentPageDemo : ContentPage
{
	private ObservableCollection<Producto> _productos;
	private Stream _selectedImageStream;
	private string _selectedImageFilePath;

	public ContentPageDemo()
	{
		InitializeComponent();
		_productos = new ObservableCollection<Producto>();
		ProductsListView.ItemsSource = _productos;
	}

	private void OnAddProductButtonClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(ProductoNombre.Text) ||
	   string.IsNullOrEmpty(ProductoCategoria.Text) ||
	   !decimal.TryParse(ProductoPrecio.Text, out var precio) ||
	   !int.TryParse(ProductoCantidad.Text, out var cantidad) ||
	   string.IsNullOrEmpty(_selectedImageFilePath))
		{
			DisplayAlert("Error", "Por favor ingresa todos los campos correctamente.", "OK");
			return;
		}

		var producto = new Producto
		{
			ImageFilePath = _selectedImageFilePath,
			Nombre = ProductoNombre.Text,
			Categoria = ProductoCategoria.Text,
			Precio = precio,
			Cantidad = cantidad
		};

		_productos.Add(producto);

		ProductsListView.ItemsSource = null;
		ProductsListView.ItemsSource = _productos;

		// Limpiar los campos
		ProductoNombre.Text = string.Empty;
		ProductoCategoria.Text = string.Empty;
		ProductoPrecio.Text = string.Empty;
		ProductoCantidad.Text = string.Empty;
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