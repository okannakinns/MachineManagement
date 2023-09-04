var $displayUser = $('.count');
var totalUser = $displayUser.data('total-user'); // .data() metoduyla attribute'den deðeri alýyoruz
function countUp(count)
{
    var div_by = 20,
        speed = Math.round(count / div_by),
        $display = $('.count'),
        run_count = 1,
        int_speed = 24;

    var int = setInterval(function() {
        if(run_count < div_by){
            $display.text(speed * run_count);
            run_count++;
        } else if(parseInt($display.text()) < count) {
            var curr_count = parseInt($display.text()) + 1;
            $display.text(curr_count);
        } else {
            clearInterval(int);
        }
    }, int_speed);
}

countUp(totalUser);

var $displayMusteri = $('.count2');
var totalMusteri = $displayMusteri.data('total-musteri'); // .data() metoduyla attribute'den deðeri alýyoruz
function countUp2(count)
{
    var div_by = 40,
        speed = Math.round(count / div_by),
        $display = $('.count2'),
        run_count = 1,
        int_speed = 24;

    var int = setInterval(function() {
        if(run_count < div_by){
            $display.text(speed * run_count);
            run_count++;
        } else if(parseInt($display.text()) < count) {
            var curr_count = parseInt($display.text()) + 1;
            $display.text(curr_count);
        } else {
            clearInterval(int);
        }
    }, int_speed);
}

countUp2(totalMusteri);

var $displayJob = $('.count3');
var totalJob = $displayJob.data('total-job'); // .data() metoduyla attribute'den deðeri alýyoruz
function countUp3(count)
{
    var div_by = 60,
        speed = Math.round(count / div_by),
        $display = $('.count3'),
        run_count = 1,
        int_speed = 24;

    var int = setInterval(function() {
        if(run_count < div_by){
            $display.text(speed * run_count);
            run_count++;
        } else if(parseInt($display.text()) < count) {
            var curr_count = parseInt($display.text()) + 1;
            $display.text(curr_count);
        } else {
            clearInterval(int);
        }
    }, int_speed);
}

countUp3(totalJob);


var $display = $('.count4');
var totalUcret = $display.data('total-ucret'); // .data() metoduyla attribute'den deðeri alýyoruz
function countUp4(count)
{
    

   
        var div_by = 100,
            speed = Math.round(count / div_by),
            run_count = 1,
            int_speed = 1;

        var int = setInterval(function () {
            if (run_count < div_by) {
                $display.text(speed * run_count);
                run_count++;
            } else if (parseInt($display.text()) < count) {
                var curr_count = parseInt($display.text()) + 1;
                $display.text(curr_count);
            } else {
                clearInterval(int);
            }
        }, int_speed);
    }

    // JavaScript tarafýnda totalUcret deðerini kullanarak fonksiyonu çaðýrýyoruz
countUp4(totalUcret);
    