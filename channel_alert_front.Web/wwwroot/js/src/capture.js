async function capture(querySelector = "body", filename = "image") {
    console.log(htmlToImage)
    const element = document.querySelector(querySelector);
    const blob = await htmlToImage.toPng(element);

    const link = document.createElement("a");
    link.href = blob;
    link.download = `${filename}.png`;
    link.click();
    link.remove();
}

async function captureJpg(querySelector = "body", filename = "image") {
    const element = document.querySelector(querySelector);
    const blob = await htmlToImage.toJpeg(element);

    const link = document.createElement("a");
    link.href = blob;
    link.download = `${filename}.jpg`;
    link.click();
    link.remove();
}

async function capturePdf() {
    const element = document.querySelector("body");

    const blob = await htmlToImage.toCanvas(element);
    const imgData = blob.toDataURL();
    const canvas = blob;
    const imgWidth = 210;
    const pageHeight = 297;
    const imgHeight = parseInt(canvas.height * imgWidth / canvas.width);
    let heightLeft = imgHeight;
    const margin = 0;
    let position = 0;
    const imageType = "png";

    /* default => p: portrait, mm: millimeters, a4: export A4 */
    const doc = new jspdf.jsPDF("p", "mm", "a4");
    doc.addImage(imgData, imageType, margin, position, imgWidth, imgHeight);
    heightLeft -= pageHeight;

    while (heightLeft >= 0) {
        position = heightLeft - imgHeight;
        doc.addPage();
        doc.addImage(imgData, imageType, margin, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
    }

    doc.save("sample.pdf");
}