using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using static Android.Views.View;

namespace tabApp.UI.Adapters.Swipe
{
    public class SwipeController : ItemTouchHelper.SimpleCallback, IOnTouchListener
    {
        public static int BUTTON_WIDTH = 200;
        private RecyclerView recyclerView;
        private Context context;
        private HomeViewModel viewModel;
        private List<UnderlayButton> buttons;
        private GestureDetectorClass gestureListner;
        private GestureDetector gestureDetector;
        private int swipedPos = -1;
        private float swipeThreshold = 0.5f;
        private Dictionary<int, List<UnderlayButton>> buttonsBuffer;
        private Queue<int> recoverQueue;

        public SwipeController(Context context, Core.HomeViewModel viewModel) : base(0, ItemTouchHelper.Left)
        {
            this.context = context;
            this.viewModel = viewModel;
            this.buttons = new List<UnderlayButton>();
            this.gestureListner = new GestureDetectorClass(this.buttons);
            this.gestureDetector = new GestureDetector(context, gestureListner);
            buttonsBuffer = new Dictionary<int, List<UnderlayButton>>();
            recoverQueue = new Queue<int>();
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int pos = viewHolder.AdapterPosition;

            if (swipedPos != pos)
                recoverQueue.Enqueue(swipedPos);

            swipedPos = pos;

            if (buttonsBuffer.ContainsKey(swipedPos))
                buttons = buttonsBuffer[swipedPos];
            else
                buttons.Clear();

            buttonsBuffer.Clear();
            swipeThreshold = 0.5f * buttons.Count * BUTTON_WIDTH;
            RecoverSwipedItem();
        }
        public override float GetSwipeThreshold(RecyclerView.ViewHolder viewHolder)
        {
            return swipeThreshold;
        }
        public override float GetSwipeEscapeVelocity(float defaultValue)
        {
            return 0.1f * defaultValue;
        }
        public override float GetSwipeVelocityThreshold(float defaultValue)
        {
            return 5.0f * defaultValue;
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            int pos = viewHolder.AdapterPosition;
            float translationX = dX;
            View itemView = viewHolder.ItemView;

            if (pos < 0)
            {
                swipedPos = pos;
                return;
            }

            if (actionState == ItemTouchHelper.ActionStateSwipe)
            {
                if (dX < 0)
                {
                    List<UnderlayButton> buffer = new List<UnderlayButton>();

                    if (!buttonsBuffer.ContainsKey(pos))
                    {
                        InstantiateUnderlayButton(viewHolder, buffer);
                        buttonsBuffer.Add(pos, buffer);
                    }
                    else
                    {
                        buffer = buttonsBuffer[pos];
                    }

                    translationX = dX * buffer.Count * BUTTON_WIDTH / itemView.Width;
                    DrawButtons(c, itemView, buffer, pos, translationX);
                }
            }

            base.OnChildDraw(c, recyclerView, viewHolder, translationX, dY, actionState, isCurrentlyActive);
        }
        private void DrawButtons(Canvas c, View itemView, List<UnderlayButton> buffer, int pos, float dX)
        {
            float right = itemView.Right;
            float dButtonWidth = (-1) * dX / buffer.Count;

            foreach (UnderlayButton button in buffer)
            {
                float left = right - dButtonWidth;
                button.OnDraw(
                        c,
                        new RectF(
                                left,
                                itemView.Top,
                                right,
                                itemView.Bottom
                        ),
                        pos,
                        context
                );

                right = left;
            }
        }
        public void InstantiateUnderlayButton(RecyclerView.ViewHolder viewHolder, List<UnderlayButton> underlayButtons) {
            underlayButtons.Add(new UnderlayButton("Apagar", Resource.Drawable.ic_delete, Color.ParseColor("#FF3C30"), viewModel.DeleteClientCommand));
            underlayButtons.Add(new UnderlayButton("Inativar", 0, Color.ParseColor("#FF9502"), viewModel.StopDailysClientCommand));
            underlayButtons.Add(new UnderlayButton("Outros", 0, Color.ParseColor("#C7C7CB"), /*viewModel.DeleteClientCommand*/null));
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (swipedPos < 0) return false;
            Point point = new Point((int)e.RawX, (int)e.RawY);

            RecyclerView.ViewHolder swipedViewHolder = recyclerView.FindViewHolderForAdapterPosition(swipedPos);
            View swipedItem = swipedViewHolder?.ItemView;
            if (swipedItem == null) return false;
            Rect rect = new Rect();
            swipedItem.GetGlobalVisibleRect(rect);

            if (e.Action == MotionEventActions.Down || e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Move)
            {
                if (rect.Top < point.Y && rect.Bottom > point.Y) {
                    gestureListner.buttons = buttons;
                    gestureDetector.OnTouchEvent(e);
                }
                else
                {
                    recoverQueue.Enqueue(swipedPos);
                    swipedPos = -1;
                    RecoverSwipedItem();
                }
            }
            return false;
        }
        private void RecoverSwipedItem()
        {
            lock (recoverQueue)
            {
                while (!(recoverQueue.Count == 0))
                {
                    int pos = recoverQueue.Dequeue();
                    if (pos > -1)
                    {
                        recyclerView?.GetAdapter().NotifyItemChanged(pos);
                    }
                }
            }
        }
        public void AttachToRecyclerView(RecyclerView recyclerView)
        {
            this.recyclerView = recyclerView;
            this.recyclerView.SetOnTouchListener(this);
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(this);
            itemTouchHelper.AttachToRecyclerView(this.recyclerView);
        }
    }

    public class GestureDetectorClass : GestureDetector.SimpleOnGestureListener
    {
        public List<UnderlayButton> buttons;

        public GestureDetectorClass(List<UnderlayButton> buttons)
        {
            this.buttons = buttons;
        }

        public override bool OnSingleTapConfirmed(MotionEvent e)
        {
            foreach (UnderlayButton button in buttons)
            {
                if (button.OnClick(e.GetX(), e.GetY()))
                    break;
            }
            return true;
        }
    }
}