doxygen Doxyfile

git checkout gh-pages
cp -r html/* .
git add .
git commit -m "Updated documatation"
git push github gh-pages
git checkout main