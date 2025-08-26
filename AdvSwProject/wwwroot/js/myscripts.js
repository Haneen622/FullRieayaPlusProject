
document.querySelectorAll('.box').forEach(function(box) {
  const circle = box.querySelector('span'); // فقط span داخل البوكس

  box.addEventListener('mousemove', function(e){
    let x = e.pageX - box.offsetLeft;
    let y = e.pageY - box.offsetTop;

    circle.style.left = x + 'px';
    circle.style.top = y + 'px';
  });
});

