﻿using System;

namespace FileOnQ.Imaging.Raw
{
	unsafe class UnpackedImage : IUnpackedImage
	{
		RawImageData data;
		IImageProcessor processor;

		internal UnpackedImage(IntPtr libraw)
		{
			data = new RawImageData
			{
				LibRawData = libraw
			};
		}

		public void Process(IImageProcessor newProcessor)
		{
			if (processor != null)
				processor.Dispose();

			processor = newProcessor;
			processor.Process(data);
		}

		public void Write(string file)
		{
			if (processor == null)
				throw new NullReferenceException("Call Process(IImageProcessor) first");

			processor.Write(data, file);
		}

		public ProcessedImage AsProcessedImage()
		{
			if (processor == null)
				throw new NullReferenceException("Call Process(IImageProcessor) first");

			return processor.AsProcessedImage(data);
		}

		~UnpackedImage() => Dispose(false);

		bool isDisposed;
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed)
				return;

			if (disposing)
			{
				// free managed resources
			}

			// free unmanaged resources
			if (processor != null)
			{
				// The processor has unmanaged memory so we are clearing it here
				// instead of in the managed area
				processor.Dispose();
				processor = null;
			}
			
			if (data != null)
			{
				// Clear pointer, but don't clear memory, let the owner clear the memory
				data.LibRawData = IntPtr.Zero;
				data = null;
			}

			isDisposed = true;
		}
	}
}
