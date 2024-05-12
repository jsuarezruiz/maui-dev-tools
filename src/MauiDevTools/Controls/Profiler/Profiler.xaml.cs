using CommunityToolkit.Maui.Views;
using MauiDevTools.Extensions;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiDevTools.Controls;

public partial class Profiler : Popup, IDisposable
{
    readonly IDispatcherTimer _timer;
    long _peakMemory;
    long _lowestMemory;
    ObservableCollection<ChartItem> _memoryItems;
    ObservableCollection<ChartItem> _cpuItems;

    public Profiler()
    {
        InitializeComponent();

        _memoryItems = new ObservableCollection<ChartItem>();
        _cpuItems = new ObservableCollection<ChartItem>();
        
        Expanded = false;

        LoadDataAsync().FireAndForget();

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += OnElapsed;
        _timer.Start();
    }

    bool Expanded { get; set; }

    async void OnElapsed(object? sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    async Task LoadDataAsync()
    {
        MemoryChart.IsVisible = CpuChart.IsVisible = Expanded;

        var usedMemory = GetMemoryUsageForProcess();

        if (usedMemory > _peakMemory)
            _peakMemory = usedMemory;

        if (_lowestMemory == 0)
            _lowestMemory = usedMemory;

        if (usedMemory < _lowestMemory)
            _lowestMemory = usedMemory;

        var usedMemoryInMB = usedMemory / (1024 * 1024);
        
        UsedMemory.Text = usedMemoryInMB.ToString("F2");

        if (Expanded)
        {
            if (_memoryItems.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                    _memoryItems.Add(new ChartItem { Label = "-", Value = usedMemoryInMB });
            }
            else
                _memoryItems.Add(new ChartItem { Label = "-", Value = usedMemoryInMB });

            if (_memoryItems.Count > 10)
                _memoryItems.RemoveAt(0);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MemoryChart.Entries = _memoryItems;
                MemoryChart.Invalidate();
            });
        }

        // Console.WriteLine("Memory, Used: {0} ({1}MB), Peak: {2}, Lowest: {3}, MaxConsumed: {4}", usedMemory, usedMemoryInMB, _peakMemory, _lowestMemory, _peakMemory - _lowestMemory);

        var cpu = await GetCpuUsageForProcess();

        UsedCpu.Text = cpu.ToString("F2");

        if (Expanded)
        {
            if (_cpuItems.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                    _cpuItems.Add(new ChartItem { Label = "-", Value = cpu });
            }
            else
                _cpuItems.Add(new ChartItem { Label = "-", Value = cpu });

            if (_cpuItems.Count > 10)
                _cpuItems.RemoveAt(0);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                CpuChart.Entries = _cpuItems;
                CpuChart.Invalidate();
            });
        }
    }

    public void Dispose()
    {
        _timer.Stop();
    }
    long GetMemoryUsageForProcess()
    {
        return Process.GetCurrentProcess().WorkingSet64;
    }
     
    async Task<float> GetCpuUsageForProcess()
    {
        var startTime = DateTime.UtcNow;
#pragma warning disable CA1416 // Validate platform compatibility
        var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
#pragma warning restore CA1416 // Validate platform compatibility
        await Task.Delay(500);

        var endTime = DateTime.UtcNow;
#pragma warning disable CA1416 // Validate platform compatibility
        var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
#pragma warning restore CA1416 // Validate platform compatibility
        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds;
        var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
        return (float)cpuUsageTotal * 100;
    }

    void ButtonTapped(object sender, TappedEventArgs e)
    {
        if(Expanded)
        {
            Expanded = false;
            ButtonText.Text = "Expand";
        }
        else
        {
            Expanded = true;
            ButtonText.Text = "Collapse";
        }
    }
}