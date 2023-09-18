namespace CS_Labs.Helpers;

public class SelectForm<T> where T : notnull {
  private readonly List<KeyValuePair<T, string>> _list = new();

  private string       _title = null!;
  private bool         _isVertical;
  private int          _width = 18;
  private ConsoleColor _fg    = ConsoleColor.Black;
  private ConsoleColor _bg    = ConsoleColor.DarkCyan;
  private int          _defaultSelectedIndex;

  public SelectForm<T> Vertical() {
    _isVertical = true;
    return this;
  }

  public SelectForm<T> Title(string title) {
    _title = title;
    return this;
  }

  public SelectForm<T> Horizontal() {
    _isVertical = false;
    return this;
  }

  public SelectForm<T> Foreground(ConsoleColor fg) {
    _fg = fg;
    return this;
  }

  public SelectForm<T> Background(ConsoleColor bg) {
    _bg = bg;
    return this;
  }

  public SelectForm<T> DefaultSelected(int index) {
    _defaultSelectedIndex = index;
    return this;
  }

  public SelectForm<T> Width(int width) {
    _width = width;
    return this;
  }

  public SelectForm<T> AddOption(T option, string text) {
    _list.Add(new KeyValuePair<T, string>(option, text));
    return this;
  }

  public T Render() {
    return GraphicHelper.SelectMultipleChoice<T>(
      _list,
      _defaultSelectedIndex,
      _isVertical,
      _bg,
      _fg,
      _title,
      _width
    );
  }
}
