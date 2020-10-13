﻿using Flurl.Util;
using MySqlX.XDevAPI.Common;
using Pflanzenbestimmung_Desktop.XAML;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pflanzenbestimmung_Desktop
{
    /// <summary>
    /// Interaktionslogik für Benutzerverwaltung.xaml
    /// </summary>
    public partial class Benutzerverwaltung : UserControl
    {
        private bool azubiReiter;
        public Benutzerverwaltung()
        {
            InitializeComponent();
            Main.LadeAzubiDaten();
            Adminliste.ItemsSource = Main.InitializeAdminVerwaltungListe();
            Azubiliste.ItemsSource = Main.InitializeAzubiVerwaltungListe();
            azubiReiter = true;
        }

        private void Zurück_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.changeContent(new Administration());
        }

        private void Bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            if (azubiReiter)
            {
                Azubis auswahl = null;
                if (Azubiliste.SelectedItem == null)
                {
                    MessageBox.Show("Niemanden ausgewählt!");
                }
                else
                {
                    for (int i = 0; i < Main.azubiVerwaltungListe.Count; i++)
                    {
                        if (Azubiliste.SelectedItem.Equals(Main.azubiVerwaltungListe[i]))
                        {
                            auswahl = Main.azubiVerwaltungListe[i];
                            break;
                        }
                    }
                    MainWindow.changeContent(new BenutzerVerwalten(auswahl));
                }
            }
            else if (!azubiReiter)
            {
                Administrator auswahl = null;
                if (Adminliste.SelectedItem == null)
                {
                    MessageBox.Show("Niemanden ausgewählt!");
                }
                else
                {
                    for (int l = 0; l < Main.AdminVerwaltungListe.Count; l++)
                    {
                        if (Adminliste.SelectedItem.Equals(Main.AdminVerwaltungListe[l]))
                        {
                            auswahl = Main.AdminVerwaltungListe[l];
                            break;
                        }
                    }
                }
            }
        }

        private void Loeschen_Click(object sender, RoutedEventArgs e)
        {
            string nachricht = null;
            Azubis bestätigen = new Azubis();
            Azubis auswahl = null;
            string azubiName = null;
            string azubiVorname = null;
            if (Azubiliste.SelectedItem == null)
            {
                MessageBox.Show("Niemanden ausgewählt!");
            }
            else
            {
                for (int i = 0; i < Main.azubiVerwaltungListe.Count; i++)
                {
                    if (Azubiliste.SelectedItem.Equals(Main.azubiVerwaltungListe[i]))
                    {
                        auswahl = Main.azubiVerwaltungListe[i];
                        break;
                    }
                }
                try
                {
                    azubiName = auswahl.Name;
                    azubiVorname = auswahl.Vorname;
                    nachricht = "Sind sie sich sicher, dass der Benutzer:\n'" + azubiName + ", "
                                     + azubiVorname + "'\n gelöscht werden soll?";
                    string caption = "Löschen?";
                    var result = MessageBox.Show(nachricht, caption, MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        Main.api_anbindung.BenutzerLoeschen(auswahl.ID);
                        MainWindow.changeContent(new Benutzerverwaltung());
                    }

                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }

        private void TabHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabHolder.SelectedIndex == 0)
            {
                azubiReiter = true;
            }
            else
            {
                azubiReiter = false;
            }
        }
    }
}
