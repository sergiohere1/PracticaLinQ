﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace LinQ
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Contacto> contactos = new ObservableCollection<Contacto>();
        ObservableCollection<Contacto> busquedaContactos;
               
        public MainPage()
        {
            InitializeComponent();
            readAndInsertContactsFromXML();
            idListaContacto.ItemsSource = contactos;


            btnReset.Clicked += (sender, args) =>
            {
                limpiarBusquedaContactos();
                searchNameBox.Text = "";
                ageBox.Text = "";
                idListaContacto.ItemsSource = contactos;
            };

            whereButton.Clicked += (sender, args) =>
            {
                limpiarBusquedaContactos();
                if (!hayErrorDeInput(1))
                {
                    wherePorNombre();
                }
            };

            btnFirstOrDef.Clicked += (sender, args) =>
            {
                limpiarBusquedaContactos();
                if (!hayErrorDeInput(2))
                {
                    firstOrDefaultPorEdad();
                }
            };

            btnSingleOrDef.Clicked += (sender, args) =>
            {
                limpiarBusquedaContactos();
                if (!hayErrorDeInput(3))
                {
                    singleOrDefMethod();
                }               
            };

            btnLastOrDefault.Clicked += (sender, args) =>
            {
                if (!hayErrorDeInput(2)) { 
                    limpiarBusquedaContactos();
                    lastOrDefaultMethod();
                }
            };

            btnOrderBy.Clicked += (sender, args) =>
            {
                limpiarBusquedaContactos();
                orderByName();

            };

            btnOrderByDescending.Clicked += (sender, args) =>
            {
                limpiarBusquedaContactos();
                orderByNameDescending();
            };

            btnSkipWhile.Clicked += (sender, args) =>
            {
                if (!hayErrorDeInput(2)) { 
                    limpiarBusquedaContactos();
                    skipWhileMayorEdad();
                }
            };

            btnTakeWhile.Clicked += (sender, args) =>
            {
                if (!hayErrorDeInput(2)) { 
                    limpiarBusquedaContactos();
                    takeWhileMayorEdad();
                }
            };
        }

        /// <summary>
        /// Método encargado de limpiar el ArrayList en el que se almacenan los resultados de las
        /// búsquedas.
        /// </summary>
        private void limpiarBusquedaContactos()
        {
            if(busquedaContactos != null)
            {
                busquedaContactos.Clear();
            }
        }

        /// <summary>
        /// Método encargado de realizar diversas comprobaciones para ver si hay algún campo de texto
        /// que esté vacío. Le pasaremos un int entre 1 y 3 que será lo que querremos comprobar si
        /// está vacío.
        /// Pasarle un 1 : Comprobar si el nombre está vacío
        /// Pasarle un 2: Comprobar si la edad está vacía
        /// Pasarle un 3: Comprobar si edad o nombre están vacías
        /// </summary>
        /// <param name="tipoComprobacion"></param>
        /// <returns></returns>
        private Boolean hayErrorDeInput(int tipoComprobacion)
        {
            bool hayError = false;
            switch (tipoComprobacion)
            {
                case 1:
                    if (string.IsNullOrEmpty(searchNameBox.Text))
                    {
                        DisplayAlert("Error", "El campo de nombre está vacío", "Aceptar");
                        hayError = true;
                        
                    }
                    break;
                case 2:
                    if (string.IsNullOrEmpty(ageBox.Text) || int.Parse(ageBox.Text) < 0) {
                        DisplayAlert("Error", "El campo de edad está vacío/es negativo", "Aceptar");
                            hayError = true;
                    }
                    break;
                case 3:
                    if(string.IsNullOrEmpty(searchNameBox.Text) || string.IsNullOrEmpty(ageBox.Text) || int.Parse(ageBox.Text) < 0) { 
                        DisplayAlert("Error", "Uno de los campos de nombre/edad está vacío o la edad es negativa", "Aceptar");
                        hayError = true;
                    }
                    break;
                }

            return hayError;
        }

        /// <summary>
        /// Método encargado de realizar la búsqueda LinQ correspondiente a FirstOrDefault, pillando el
        /// primer contacto que tenga la edad buscada o el default.
        /// </summary>
        private void firstOrDefaultPorEdad()
        {
            var contactoResultante = contactos.FirstOrDefault(edad => edad.Edad == ageBox.Text);
            if(contactoResultante != null) {
                busquedaContactos = new ObservableCollection<Contacto>();
                busquedaContactos.Add(contactoResultante);
            }
            else
            {
                DisplayAlert("", "La busqueda no encontró ningún contacto con esa edad", "Aceptar");
            }
            idListaContacto.ItemsSource = busquedaContactos;
        }

        /// <summary>
        /// Método encargado de buscar a todos los contactos cuyo nombre coincida con el que se
        /// ha introcido en el Entry del Nombre
        /// </summary>
        private void wherePorNombre()
        {
            var busquedaResultante = contactos.Where(p => p.Nombre.Equals(searchNameBox.Text)).ToList();
            busquedaContactos = new ObservableCollection<Contacto>(busquedaResultante);
            idListaContacto.ItemsSource = busquedaContactos;
        }

        /// <summary>
        /// Método encargado de realizar la búsqueda de SingleOrDefault en la Lista de Contactos,
        /// filtrando por nombre y seleccionando por edad.
        /// </summary>
        private void singleOrDefMethod()
        {
            var contactoResultante = contactos.Where(c => c.Nombre.Contains(searchNameBox.Text)).SingleOrDefault(c => c.Edad == ageBox.Text);
            if (contactoResultante != null)
            {
                busquedaContactos = new ObservableCollection<Contacto>();
                busquedaContactos.Add(contactoResultante);
            }
            else
            {
                DisplayAlert("", "La busqueda no encontró ningún contacto con esa edad", "Aceptar");
            }
            idListaContacto.ItemsSource = busquedaContactos;
        }

        /// <summary>
        /// Método encargado de realizar el LastOrDefault (Obtener el último contacto) que coincida
        /// con la edad introducida por el usuario.
        /// </summary>
        private void lastOrDefaultMethod()
        {
            var contactoResultante = contactos.LastOrDefault(c => c.Edad == ageBox.Text);
            if (contactoResultante != null)
            {
                busquedaContactos = new ObservableCollection<Contacto>();
                busquedaContactos.Add(contactoResultante);
            }
            else
            {
                DisplayAlert("", "La busqueda no encontró ningún contacto con esa edad", "Aceptar");
            }
            idListaContacto.ItemsSource = busquedaContactos;
        }

        /// <summary>
        /// Método encargado de ordenar de manera Ascendente por nombre la lista de Contactos
        /// </summary>
        private void orderByName()
        {
            var busquedaResultante = contactos.OrderBy(c => c.Nombre).ToList();
            busquedaContactos = new ObservableCollection<Contacto>(busquedaResultante);
            if (busquedaContactos.Count == 0)
            {
                DisplayAlert("", "La búsqueda no encontró ningún contacto que cumpla esos criterios", "Aceptar");
            }
            idListaContacto.ItemsSource = busquedaContactos;
        }

        /// <summary>
        /// Método encargado de ordenar de manera descendente y por nombre la lista de contactos.
        /// </summary>
        private void orderByNameDescending()
        {
            var busquedaResultante = contactos.OrderByDescending(c => c.Nombre).ToList();
            busquedaContactos = new ObservableCollection<Contacto>(busquedaResultante);
            if (busquedaContactos.Count == 0)
            {
                DisplayAlert("", "La búsqueda no encontró ningún contacto que cumpla esos criterios", "Aceptar");
            }
            idListaContacto.ItemsSource = busquedaContactos;
        }

        /// <summary>
        /// Método encargado de saltarse todos aquellos contactos que sean mayores a la edad introducida
        /// hasta que esa condición ya no se cumpla.
        /// </summary>
        private void skipWhileMayorEdad()
        {
            var busquedaResultante = contactos.SkipWhile(c => int.Parse(c.Edad) > int.Parse(ageBox.Text)).ToList();
            busquedaContactos = new ObservableCollection<Contacto>(busquedaResultante);
            if(busquedaContactos.Count == 0)
            {
                DisplayAlert("", "La búsqueda no encontró ningún contacto que cumpla esos criterios", "Aceptar");
            }
            idListaContacto.ItemsSource = busquedaContactos;
        }

        /// <summary>
        /// Método encargado de coger todos aquellos contactos que sean mayores a la edad introducida
        /// hasta que esa condición ya no se cumpla.
        /// </summary>
        private void takeWhileMayorEdad()
        {
            var busquedaResultante = contactos.TakeWhile(c => int.Parse(c.Edad) > int.Parse(ageBox.Text)).ToList();
            busquedaContactos = new ObservableCollection<Contacto>(busquedaResultante);
            if (busquedaContactos.Count == 0)
            {
                DisplayAlert("", "La búsqueda no encontró ningún contacto que cumpla esos criterios", "Aceptar");
            }
            idListaContacto.ItemsSource = busquedaContactos;
        }


        /// <summary>
        /// Método encargado de realizar la lectura del XML e insertarlo en la ObservableCollection
        /// de contactos.
        /// </summary>
        private void readAndInsertContactsFromXML()
        {
            string ruta = "LinQ.Assets.Info.xml";
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(ruta);
            var doc = XDocument.Load(stream);

            foreach (XElement element in doc.Root.Elements())
            {
                contactos.Add(new Contacto(element.Element("NOMBRE").Value, int.Parse(element.Element("EDAD").Value.ToString()), element.Element("DNI").Value));
            }
        }



        
    }
}
