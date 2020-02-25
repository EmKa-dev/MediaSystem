﻿using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF;
using MediaSystem.DesktopClientWPF.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for MusicViewer.xaml
    /// </summary>
    public partial class MusicViewer : Window
    {
        public MusicViewer(List<MediaFileInfo> files, IPEndPoint ipEnd = null)
        {
            InitializeComponent();

            this.DataContext = new MusicPlayerViewModel(files, ipEnd);

            var resizer = new WindowResizer(this);
        }
    }
}
