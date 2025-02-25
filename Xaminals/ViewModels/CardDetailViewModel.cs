using MagicScannerLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Xaminals.ViewModels
{
    public class CardDetailViewModel : IQueryAttributable, INotifyPropertyChanged
	{
		public Card Card { get; private set; }

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			Card = query["Card"] as Card;
			OnPropertyChanged("Card");
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
