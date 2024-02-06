import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';

const AdminDisksAdd = () => {
    let { id } = useParams();
    const [name, setName] = useState("");
    const [price, setPrice] = useState(0);
    const [leftOnStock, setLeftOnStock] = useState(0);
    const [rating, setRating] = useState(0);
    const [coverImagePath, setCoverImagePath] = useState("");
    const [imageStorageName, setImageStorageName] = useState("");
    const [message, setMessage] = useState("");

    function validateForm() {
        return name.length > 0 && price > 0 && leftOnStock > 0 && rating >= 0 && rating <= 5 && coverImagePath.length > 0 && imageStorageName.length > 0
    }

    let handleSubmit = async (e) => {
        e.preventDefault();
        console.log(JSON.stringify({
            name: name,
            price: price,
            leftOnStock: leftOnStock,
            rating: rating,
            coverImagePath: coverImagePath,
            imageStorageName: imageStorageName,
          }));
        try {
          if (!validateForm()) {setMessage("Please fill all the fields with valid values"); return;}
          let identifier = id.split("=")[1];
          console.log(identifier);
          let url="https://localhost:7117/Discs/Edit?"+"existingDiscId="+identifier+"&"+id+"&Name="+name+"&Price="+price+"&LeftOnStock="+leftOnStock+"&Rating="+rating+"&CoverImagePath="+coverImagePath+"&ImageStorageName="+imageStorageName;
          console.log(url);
          let res = await fetch(url, {
            method: "PUT"
          });
          let resJson = await res.json();
          if (res.status === 200) {
            
            setMessage("Disk created successfully with id=" + resJson.id);
          } else {
            setMessage("Some error occured");
          }
        } catch (err) {
          console.log(err);
        }
      };
      useEffect(() => {
        console.log(`https://localhost:7117/Discs/GetDisc?${id}`);
        fetch(`https://localhost:7117/Discs/GetDisc?${id}`).then(res => res.json()).then(data => {
            setName(data.name);
            setPrice(data.price);
            setLeftOnStock(data.leftOnStock);
            setRating(data.rating);
            setCoverImagePath(data.coverImagePath);
            setImageStorageName(data.imageStorageName);
        }); 
      },[id])
    
    return (
        <>
            <AdminPanelHeader />
            <div className='admindisks'>
                <h1>Edit disk</h1>
                <form onSubmit={handleSubmit}>
                    <input type="text" name="name" id="name" placeholder='Name' value= {name} onChange={(e) => setName(e.target.value)}/>
                    <input type="number" step=".01" name="price" id="price" placeholder='Price' value={price} onChange={(e) => setPrice(e.target.value)}/>
                    <input type="number" step="1" min="1" name="leftOnStock" id="leftOnStock" value={leftOnStock} placeholder='Left on stock' onChange={(e) => setLeftOnStock(e.target.value)}/>
                    <input type="number" step="1" min="1" max="5" name="rating" id="rating" placeholder='Rating' value={rating} onChange={(e) => setRating(e.target.value)}/>
                    <input type="text" name="coverImagePath" id="coverImagePath" placeholder='Cover image path' value={coverImagePath} onChange={(e) => setCoverImagePath(e.target.value)}/>
                    <input type="text" name="imageStorageName" id="imageStorageName" placeholder='Image storage name' value={imageStorageName} onChange={(e) => setImageStorageName(e.target.value)}/>

                    <button type="submit">Edit</button>

                    <div className="message">{message ? <p>{message}</p> : null}</div>
                </form>
                
            </div>
        </>
    )
}

export default AdminDisksAdd
