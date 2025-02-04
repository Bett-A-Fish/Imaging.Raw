﻿using System;
using System.Runtime.InteropServices;

namespace FileOnQ.Imaging.Raw
{
	public partial class LibRaw
	{
		private static unsafe class X64
		{
			const string DllName = "libraw_fileonq.dll";

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr libraw_init(uint flags);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void libraw_close(IntPtr libraw);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void libraw_recycle(IntPtr libraw);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Error libraw_open_file(IntPtr libraw, string filename);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Error libraw_unpack_thumb(IntPtr libraw);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Error libraw_dcraw_thumb_writer(IntPtr libraw, string filename);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern ProcessedImage* libraw_dcraw_make_mem_thumb(IntPtr libraw, ref Error errorCode);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern ProcessedImage* libraw_dcraw_make_mem_image(IntPtr libraw, ref Error errorCode);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Error libraw_unpack(IntPtr libraw);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Error libraw_dcraw_process(IntPtr libraw);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void libraw_set_output_tif(IntPtr libraw, int value);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void libraw_dcraw_ppm_tiff_writer(IntPtr libraw, string filename);

			// This must pass the processed image so the native lib knows what to clear
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void libraw_dcraw_clear_mem(IntPtr image);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern DcrawPostProcessingParameters* libraw_get_output_params(IntPtr libraw);
		}
	}
}
