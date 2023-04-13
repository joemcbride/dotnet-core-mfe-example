module.exports = {
  content: ['./*.{html,js}', './components/*.{html,js}', './Jobs/*.{html,js}'],
  theme: {
    extend: {},
  },
  variants: {},
  plugins: [require('@tailwindcss/forms'), require('@tailwindcss/typography')],
}
