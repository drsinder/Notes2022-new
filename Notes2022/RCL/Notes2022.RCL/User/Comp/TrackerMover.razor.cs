using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using Syncfusion.Blazor.SplitButtons;
using System.Net.Http.Json;

namespace Notes2022.RCL.User.Comp
{
    /// <summary>
    /// Moves traker items (Sequencer items) up or down...
    /// </summary>
    public partial class TrackerMover
    {
        /// <summary>
        /// Who are we
        /// </summary>
        [Parameter] public Sequencer CurrentTracker { get; set; }

        /// <summary>
        /// List of trackers
        /// </summary>
        [Parameter] public List<Sequencer> Trackers { get; set; }

        /// <summary>
        /// Our container/caller
        /// </summary>
        [Parameter] public Tracker Tracker { get; set; }

        /// <summary>
        /// List of items before me
        /// </summary>
        List<Sequencer> befores { get; set; }

        /// <summary>
        /// List of items after me
        /// </summary>
        List<Sequencer> afters { get; set; }

        /// <summary>
        /// Item just before me
        /// </summary>
        Sequencer before { get; set; }

        /// <summary>
        /// Item just after me
        /// </summary>
        Sequencer after { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            // find before and after items
            if (CurrentTracker is not null)
            {
                befores = Trackers.Where(p => p.Ordinal < CurrentTracker.Ordinal).OrderByDescending(p => p.Ordinal).ToList();
                if (befores is not null && befores.Count > 0)
                    before = befores.First();

                afters = Trackers.Where(p => p.Ordinal > CurrentTracker.Ordinal).OrderBy(p => p.Ordinal).ToList();
                if (afters is not null && afters.Count > 0)
                    after = afters.First();
            }
        }

        /// <summary>
        /// Move an item as wished
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task ItemSelected(MenuEventArgs args)
        {

            switch (args.Item.Text)
            {
                case "Up":
                    if (before is null)
                        return;

                    await Swap(before, CurrentTracker);

                    break;

                case "Down":
                    if (after is null)
                        return;
                    await Swap(after, CurrentTracker);

                    break;

                case "Top":
                    if (before is null)
                        return;
                    await Swap(befores[befores.Count - 1], CurrentTracker);
                    break;

                case "Bottom":
                    if (after is null)
                        return;
                    await Swap(afters[afters.Count - 1], CurrentTracker);

                    break;

                default:
                    break;
            }

            await Tracker.Shuffle();

        }

        /// <summary>
        /// Swap the postion of two trackers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private async Task Swap(Sequencer a, Sequencer b)
        {
            int aord = a.Ordinal;
            int bord = b.Ordinal;

            a.Ordinal = bord;
            b.Ordinal = aord;

            await DAL.UpateSequencerPosition(Http, a);
            await DAL.UpateSequencerPosition(Http, b);

        }
    }
}
