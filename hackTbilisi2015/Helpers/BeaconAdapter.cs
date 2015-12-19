using System;
using System.Collections.Generic;
using hackTbilisi2015.Core.BusinessObjects;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.App;

namespace hackTbilisi2015
{
	public class BeaconAdapter : BaseAdapter<iBeacon>
	{
		private List<iBeacon> _source;
		private Activity _context;

		public BeaconAdapter (List<iBeacon> source, Activity context)
		{
			_source = source;
			_context = context;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var item = _source [position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = _context.LayoutInflater.Inflate (Resource.Layout.BeaconRowLayout, null);
			view.FindViewById<TextView> (Resource.Id.beaconName).Text = item.Name;
			view.FindViewById<TextView> (Resource.Id.uuid).Text = item.UUID;
			view.FindViewById<TextView> (Resource.Id.macAddress).Text = item.MacAddress;
			return view;
		}

		public override int Count {
			get {
				return _source.Count;
			}
		}


		public override iBeacon this [int index] {
			get { return _source [index]; }
		}


	}
}

