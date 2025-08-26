/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./*.html",
    "./*.js",
    "./src/**/*.{html,js}",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Open Sans", "Noto Kufi Arabic", "Arial", "sans-serif"],
      },
    },
  },
  darkMode: 'class',
  plugins: [],
};
