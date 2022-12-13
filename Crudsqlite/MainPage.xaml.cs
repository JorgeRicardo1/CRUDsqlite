using Crudsqlite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Crudsqlite
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            llenarDatos();
        }

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            if (validarDatos())
            {
                Alumno alumno = new Alumno
                {
                    Nombre= txtNombre.Text,
                    ApellidoMaterno= txtApellidoMaterno.Text,
                    ApellidoPaterno= txtApellidoPaterno.Text,
                    Edad= int.Parse(txtEdad.Text),
                    Email= txtEmail.Text
                };
                await App.SQLiteDB.SaveAlumnoAsync(alumno);
                

                await DisplayAlert("Registro", "Se ha agregado el alumno","Ok");
                limpiarControles();
                llenarDatos();
            }
            else
            {
                await DisplayAlert("Advertencia", "Ingrese todos los datos", "ok"); 
            }
        }
        public async void llenarDatos()
        {
            var alumnoList = await App.SQLiteDB.GetAlumnosAsync();
            if (alumnoList != null)
            {
                lstAlumnos.ItemsSource = alumnoList;
            }
        }

        public bool validarDatos()
        {
            bool respuesta;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                respuesta = false;
            }
            else if (string.IsNullOrEmpty(txtApellidoMaterno.Text))
            {
                respuesta = false;
            }
            else if (string.IsNullOrEmpty(txtApellidoPaterno.Text))
            {
                respuesta = false;
            }
            else if (string.IsNullOrEmpty(txtEdad.Text))
            {
                respuesta = false;
            }
            else if (string.IsNullOrEmpty(txtEmail.Text))
            {
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return respuesta;
        }

        private async void btnActualizar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdAlumno.Text))
            {
                Alumno alumno = new Alumno()
                {
                    IdAlumno = Convert.ToInt32(txtIdAlumno.Text),
                    Nombre = txtNombre.Text,
                    ApellidoMaterno = txtApellidoMaterno.Text,
                    ApellidoPaterno = txtApellidoPaterno.Text,
                    Edad = Convert.ToInt32(txtEdad.Text),
                    Email = txtEmail.Text
                };

                await App.SQLiteDB.SaveAlumnoAsync(alumno);
                await DisplayAlert("registro", "se actualizo de manrea exitosa", "ok");

                limpiarControles();
                txtIdAlumno.IsVisible = false;
                btnActualizar.IsVisible = false;
                btnRegistrar.IsVisible = true;
                llenarDatos();
            }
        }

        private async void lstAlumnos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (Alumno)e.SelectedItem;
            btnRegistrar.IsVisible = false;
            txtIdAlumno.IsVisible = true;
            btnActualizar.IsVisible = true;
            btnEliminar.IsVisible = true;

            if (!string.IsNullOrEmpty(obj.IdAlumno.ToString()))
            {
                var alumno = await App.SQLiteDB.GetAlumnoById(obj.IdAlumno);
                if(alumno != null)
                {
                    txtIdAlumno.Text = alumno.IdAlumno.ToString();
                    txtNombre.Text = alumno.Nombre;
                    txtApellidoMaterno.Text = alumno.ApellidoMaterno;
                    txtApellidoPaterno.Text = alumno.ApellidoPaterno;
                    txtEmail.Text = alumno.Email;
                    txtEdad.Text = alumno.Edad.ToString();
                }
            }
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            var alumno = await App.SQLiteDB.GetAlumnoById(Convert.ToInt32(txtIdAlumno.Text));
            if (alumno != null)
            {
                await App.SQLiteDB.DeleteAlumno(alumno);
                await DisplayAlert("Alumno", "Se borro el alumno", "ok");
                limpiarControles();
                llenarDatos();
                txtIdAlumno.IsVisible = false;
                btnEliminar.IsVisible = false;
                btnActualizar.IsVisible = false;
                btnRegistrar.IsVisible = true;
            }
        }

        public void limpiarControles()
        {
            txtIdAlumno.Text = "";
            txtNombre.Text = "";
            txtApellidoMaterno.Text = "";
            txtApellidoPaterno.Text = "";
            txtEdad.Text = "";
            txtEmail.Text = "";
        }
    }
}
