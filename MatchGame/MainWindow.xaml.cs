using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame

{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer(); // initalize timer
        int tenthsOfSecondsElapsed; //keep track of time elapsed
        int matchesFound; // count of matches found
        int bestTime = 1000; //best time starts at one minute, overwritten by new best times

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");

            // game ends at 8 matches found
            if (matchesFound == 8)
            {
                // stop timer and overwrite new best time if applicable
                timer.Stop();
                if (tenthsOfSecondsElapsed < bestTime)
                {
                    bestTime = tenthsOfSecondsElapsed;
                }
                timeTextBlock.Text = timeTextBlock.Text + " - play again?\n" + "best time: " + (bestTime / 10F) + "s";
            }
        }

        // This method sets up the game screen by using the list of animal emojis 
        // to fill each of the 16 available grid squares
        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🦊","🦊",
                "🐵","🐵",
                "🐸","🐸",
                "🦓","🦓",
                "🐔","🐔",
                "🐪","🐪",
                "🦘","🦘",
                "🐳","🐳",

            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        // Clicking on an emoji in a grid square changes visibility to hidden
        // once a second emoji is clicked, the visibility of both emojis is hidden
        // if they match or visibility is set back to visible if they do not match
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        // Click on the bottom row of grid to restart game once all matches are found
        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
