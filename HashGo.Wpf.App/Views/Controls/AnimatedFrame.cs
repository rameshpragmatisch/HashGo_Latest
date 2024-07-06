using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows;

namespace HashGo.Wpf.App.Views.Controls
{
    public class AnimatedFrame : Frame
    {
        private bool IsAnimating { get; set; }
        private UIElement NextContent { get; set; }
        private UIElement PreviousContent { get; set; }
        private Action PreviousContentTransformCleanupDelegate { get; set; }
        private Action NextContentTransformCleanupDelegate { get; set; }

        public AnimatedFrame() => this.Navigating += OnNavigating;

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (this.IsAnimating
              || !(e.Content is UIElement nextContent
                && this.Content is UIElement))
            {
                return;
            }

            e.Cancel = true;
            this.PreviousContent = this.Content as UIElement;
            this.NextContent = nextContent;
            AnimateToNextContent();
        }

        private void AnimateToNextContent()
        {
            PrepareAnimation();
            StartPreviousContentAnimation();
        }

        private void PrepareAnimation()
        {
            this.IsAnimating = true;


            Transform originalPreviousContentTransform = this.PreviousContent.RenderTransform;
            this.PreviousContent.RenderTransform = new TranslateTransform(0, 0);
            this.PreviousContentTransformCleanupDelegate =
              () => this.PreviousContent.RenderTransform = originalPreviousContentTransform;


            Transform originalNextContentTransform = this.NextContent.RenderTransform;
            this.NextContent.RenderTransform = new TranslateTransform(0, 0);
            this.NextContentTransformCleanupDelegate = () => this.NextContent.RenderTransform = originalNextContentTransform;
        }

        private void StartPreviousContentAnimation()
        {
            var unloadAnimation = new Storyboard();
            DoubleAnimation slideOutAnimation = CreateSlideOutAnimation();
            unloadAnimation.Children.Add(slideOutAnimation);

            DoubleAnimation fadeOutAnimation = CreateFadeOutAnimation();
            unloadAnimation.Children.Add(fadeOutAnimation);
            unloadAnimation.Completed += StartNextContentAnimation_OnPreviousContentAnimationCompleted;

            unloadAnimation.Begin();
        }

        private void StartNextContentAnimation_OnPreviousContentAnimationCompleted(object sender, EventArgs e)
        {
            this.Content = this.NextContent;

            var loadAnimation = new Storyboard();
            DoubleAnimation slideInAnimation = CreateSlideInAnimation();
            loadAnimation.Children.Add(slideInAnimation);

            DoubleAnimation fadeInAnimation = CreateFadeInAnimation();
            loadAnimation.Children.Add(fadeInAnimation);
            loadAnimation.Completed += Cleanup_OnAnimationsCompleted;

            loadAnimation.Begin();
        }

        private void Cleanup_OnAnimationsCompleted(object sender, EventArgs e)
        {
            this.PreviousContentTransformCleanupDelegate.Invoke();
            this.NextContentTransformCleanupDelegate.Invoke();
            this.IsAnimating = false;
        }

        private DoubleAnimation CreateFadeOutAnimation()
        {
            var fadeOutAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(250)), FillBehavior.HoldEnd)
            { BeginTime = TimeSpan.Zero };
            Storyboard.SetTarget(fadeOutAnimation, this.PreviousContent);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(nameof(UIElement.Opacity)));
            return fadeOutAnimation;
        }

        private DoubleAnimation CreateSlideOutAnimation()
        {
            var slideOutAnimation = new DoubleAnimation(
                0,
                -50,
                new Duration(TimeSpan.FromMilliseconds(250)),
                FillBehavior.HoldEnd)
            { BeginTime = TimeSpan.Zero };

            Storyboard.SetTarget(slideOutAnimation, this.PreviousContent);
            Storyboard.SetTargetProperty(
              slideOutAnimation,
              new PropertyPath(
                $"{nameof(UIElement.RenderTransform)}.({nameof(TranslateTransform)}.{nameof(TranslateTransform.X)})"));
            return slideOutAnimation;
        }

        private DoubleAnimation CreateFadeInAnimation()
        {
            var fadeInAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(250)), FillBehavior.HoldEnd);
            Storyboard.SetTarget(fadeInAnimation, this.NextContent);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(nameof(UIElement.Opacity)));
            return fadeInAnimation;
        }

        private DoubleAnimation CreateSlideInAnimation()
        {
            var slideInAnimation = new DoubleAnimation(
              -50,
              0,
              new Duration(TimeSpan.FromMilliseconds(250)),
              FillBehavior.HoldEnd);

            Storyboard.SetTarget(slideInAnimation, this.NextContent);
            Storyboard.SetTargetProperty(
              slideInAnimation,
              new PropertyPath(
                $"{nameof(UIElement.RenderTransform)}.({nameof(TranslateTransform)}.{nameof(TranslateTransform.X)})"));
            return slideInAnimation;
        }
    }
}
