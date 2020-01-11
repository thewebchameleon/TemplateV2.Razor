/// <binding BeforeBuild='build-css, build-scripts, build-scripts-framework, build-scripts-demo-charts' />
let gulp = require("gulp"),
    sass = require("gulp-sass"),
    autoprefixer = require("gulp-autoprefixer"),
    rename = require("gulp-rename"),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify-es').default,
    sourcemaps = require('gulp-sourcemaps');

gulp.task('build-css', function () {
    return gulp
        .src([
            './wwwroot/sass/framework.scss',
            './wwwroot/sass/site.scss'
        ])
        .pipe(sourcemaps.init())
        .pipe(sass({
            outputStyle: "compressed"
        }).on('error', sass.logError))
        .pipe(autoprefixer())
        .pipe(rename(function (path) {
            path.extname = ".min.css";
        }))
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest("./wwwroot/css"));
});

gulp.task('build-scripts-framework', function () {
    return gulp
        .src([
            './node_modules/jquery/dist/jquery.min.js',
            './node_modules/jquery.easing/jquery.easing.min.js',
            './node_modules/jquery-validation/dist/jquery.validate.min.js',
            './node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
            './node_modules/bootstrap/dist/js/bootstrap.bundle.min.js',
            './node_modules/bootstrap-select/dist/js/bootstrap-select.min.js',
            './node_modules/chart.js/dist/Chart.min.js',
            './node_modules/startbootstrap-sb-admin-2/js/sb-admin-2.min.js',
            './wwwroot/lib/datatables/jquery.dataTables.min.js',
            './wwwroot/lib/datatables/dataTables.bootstrap4.min.js',
            './node_modules/datatables.net-fixedcolumns/js/dataTables.fixedColumns.min.js',
            './node_modules/moment/min/moment.min.js',
            './node_modules/pretty-print-json/dist/pretty-print-json.min.js',
            './node_modules/countdown/countdown.js',
            './wwwroot/lib/tempusdominus-bootstrap-4/tempusdominus-bootstrap-4.min.js',
            './node_modules/webfontloader/webfontloader.js'
        ])
        .pipe(concat('framework.min.js'))
        .pipe(gulp.dest('./wwwroot/js'));
});

gulp.task('build-scripts', function () {
    return gulp
        .src([
            './wwwroot/js/site.js'
        ])
        .pipe(sourcemaps.init())
        .pipe(concat('site.min.js'))
        .pipe(uglify())
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest('./wwwroot/js'));
});

gulp.task('build-scripts-demo-charts', function () {
    return gulp
        .src([
            './node_modules/startbootstrap-sb-admin-2/js/demo/chart-area-demo.js',
            './node_modules/startbootstrap-sb-admin-2/js/demo/chart-pie-demo.js',
            './node_modules/startbootstrap-sb-admin-2/js/demo/chart-bar-demo.js'
        ])
        .pipe(sourcemaps.init())
        .pipe(concat('demo-charts.min.js'))
        .pipe(uglify())
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest('./wwwroot/js'));
});