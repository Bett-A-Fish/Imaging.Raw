﻿using System;
using System.Runtime.InteropServices;

namespace FileOnQ.Imaging.Raw
{
	internal partial class Cuda
	{
		private unsafe class x64
		{
			const string DllName = "FileOnQ.Imaging.Raw.Gpu.Cuda.dll";

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool is_cuda_capable();

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr process_bitmap(IntPtr data, int size, int width, int height, ref int length, ref Error error);
			
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void free_memory(IntPtr pointer);
		}
	}
}
