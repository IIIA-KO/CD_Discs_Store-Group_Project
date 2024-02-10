import React, { useState } from 'react'
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';
import './admindisksadd.css';

const AdminDisksAdd = () => {
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
          let res = await fetch("https://localhost:7117/Discs/Create?Name="+name+"&Price="+price+"&LeftOnStock="+leftOnStock+"&Rating="+rating+"&CoverImagePath="+coverImagePath+"&ImageStorageName="+imageStorageName, {
            method: "POST"
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
    
    return (
        <>
            <AdminPanelHeader />
            <div className='adminadd'>
                <h1>Create disk</h1>
                <form onSubmit={handleSubmit}>
                    <input type="text" name="name" id="name" placeholder='Name' onChange={(e) => setName(e.target.value)}/>
                    <input type="number" step=".01" name="price" id="price" placeholder='Price' onChange={(e) => setPrice(e.target.value)}/>
                    <input type="number" step="1" min="1" name="leftOnStock" id="leftOnStock" placeholder='Left on stock' onChange={(e) => setLeftOnStock(e.target.value)}/>
                    <input type="number" step="1" min="1" max="5" name="rating" id="rating" placeholder='Rating' onChange={(e) => setRating(e.target.value)}/>
                    <input type="text" name="coverImagePath" id="coverImagePath" placeholder='Cover image path' onChange={(e) => setCoverImagePath(e.target.value)}/>
                    <input type="text" name="imageStorageName" id="imageStorageName" placeholder='Image storage name' onChange={(e) => setImageStorageName(e.target.value)}/>

                    <button type="submit">Create</button>

                    <div className="message">{message ? <p>{message}</p> : null}</div>
                </form>
                
            </div>
        </>
    )
}

export default AdminDisksAdd
