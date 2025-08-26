const tasbe7at = ["سبحـان الله", "الحمد لله", "لا إله إلا الله", "الله أكبر"];
let clicks = 10;
let n = 0;
let tries = 0;
let score = 0;

function count() {
  score++;
  if (tries < tasbe7at.length) {
    if (clicks > 1) {
      document.getElementById("score").innerHTML = score;
      clicks--;
      document.getElementById("text").innerHTML = clicks;
    } else {
      document.getElementById("text").innerHTML = "10";
      clicks = 10;
      document.getElementById("tasbe7").innerHTML = tasbe7at[n];
      tries++;
      n++;
    }
  } else {
    n = 0;
    tries = 0;
  }
}
