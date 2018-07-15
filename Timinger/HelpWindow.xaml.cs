﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Timinger
{
    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private List<TopicClass> topic_list = new List<TopicClass>();
        private string selected;
        public HelpWindow()
        {
            InitializeComponent();
            TopicListBox.ItemsSource = Timinger.Language.topics;
        }

        private void TopicListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = ((KeyValuePair<string,string>)(TopicListBox.SelectedItem)).Value;
            if(selected != null)
            ContentBlock.Document = (FlowDocument)this.FindResource(selected);
        }
    }
}
