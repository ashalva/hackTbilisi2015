
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using hackTbilisi2015.Activities;

namespace hackTbilisi2015
{
	public class BeaconQuantityFragment : DialogFragment
	{
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.QuantityLayout, null);
			var qButton = view.FindViewById<Button> (Resource.Id.quantity_Button);
			var qEditText = view.FindViewById<EditText> (Resource.Id.quantity);
			qButton.Click += (sender, e) => {
				if (!string.IsNullOrEmpty (qEditText.Text)) {
					(this.Activity as MainActivity).BeaconQuantity = Convert.ToInt32 (qEditText.Text);
					this.Dismiss ();
					this.Dispose ();
				} else {
					Toast.MakeText (this.Activity, "გთხოვთ შეავსოთ ველი", ToastLength.Short).Show ();
				}
			};


			return view;
		}
	}
}

