













//export function preload() {

//    let modelURL = 'https://teachablemachine.withgoogle.com/models/S4O5iAMtu/';
//    let classifier = ml5.imageClassifier(modelURL + 'model.json');
//}


//export function setup() {
//    let video;
//        createCanvas(640, 520);

//        video = createCapture(VIDEO);
//        video.hide();

//        classifyVideo();
//}


//export function classifyVideo() {
//    classifier.classify(video, gotResults);
//}

//export function draw() {
//    let accesLabel = document.getElementById("accesLabel");
//    let label = "waiting...";
//    var accessGranted = false;
//    var ProvideInfo = false;
//    background(0);

//    // Draw the video
//    image(video, 0, 0);

//    // STEP 4: Draw the label
//    textSize(32);
//    textAlign(CENTER, CENTER);
//    fill(255);
//    text(label, width / 2, height - 16);


//    let emoji = "";
//    if (label == "ring") {
//        emoji = "Ring Approved";
//        accessGranted = true;
//    } else if (label == "Blank") {
//        // emoji = "access denied";
//    } else if (label == "Art") {
//        //  emoji= "access granted";
//        ProvideInfo = true;
//    }
//    else if (label == "Wrong ring") {
//        // emoji = "access denied";
//    }
//    if (label == "No ring") {
//        accesLabel.innerText = "wrong code try again";
//    }


//    if (accessGranted == true) {
//        accesLabel.innerText = "Page unlocked";
//    }
//    else if (ProvideInfo == true) {
//        accesLabel.innerText = "Art pls provide Proof";
//    }

//    textSize(20);
//    text(emoji, width / 2, height / 2);
//}


//export function gotResults(error, results) {
//    let confidence = "score...";
//    if (error) {
//        console.error(error);
//        return;
//    }

//    label = results[0].label;
//    confidence = results[0].confidence;
//    classifyVideo();
//}
