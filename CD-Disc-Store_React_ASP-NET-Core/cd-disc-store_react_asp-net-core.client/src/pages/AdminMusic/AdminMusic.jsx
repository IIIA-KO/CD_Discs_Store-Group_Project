import React from 'react'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader'
import './../AdminDisks/admindisks.css';


const AdminMusic = () => {
    const [items, setItems] = useState([]);
    const [currentPage, setCurrentPage] = useState(0);
    let navigate=useNavigate();
    function handlePagination(pageNumber, itemsPerPage) {
        console.log(pageNumber);
        console.log(itemsPerPage);
        if (pageNumber)
        if (pageNumber < 0) {
            handlePagination(pageNumber+1, itemsPerPage);
            document.getElementById("leftarrow").disabled = true;
            return;
        } else{
            document.getElementById("leftarrow").disabled = false;
        }
        let fetchurl = "https://localhost:7117/Music/GetAll?skip=" + pageNumber * itemsPerPage;
        fetch(fetchurl)
        .then(res => res.json())
        .then(data => {
            //console.log(data.length);
            if (data.length == 0&&pageNumber>0) {
                handlePagination(pageNumber-1, itemsPerPage);
                document.getElementById("rightarrow").disabled = true;
            } else if (data.length>0){
                document.getElementById("rightarrow").disabled = false;
            }
            if(data.length>itemsPerPage){
                data=data.slice(0, itemsPerPage);
            }
            setItems(data);
        })
        .catch(error => console.error(error));
        setCurrentPage(pageNumber);
    }

    useEffect(() => {
        handlePagination(currentPage, 10);
    }, [])

    return (
        <>
            <AdminPanelHeader />
            <div className='admindisks'>
                <h1>Music</h1>
                <button className='add' onClick={() => window.location.href = "/adminpanel/music/add"}>Add</button>
                <table className='table'>
                    <thead>
                        <tr>
                            <th scope="col">ID</th>
                            <th scope="col">Name</th>
                            <th scope="col">Genre</th>
                            <th scope="col">Artist</th>
                            <th scope="col">Language</th>
                            <th scope="col">Cover Image</th>
                            <th scope="col">Image name</th>
                            <th scope="col">Edit</th>
                            <th scope="col">Delete</th>
                        </tr>
                    </thead>
                    <tbody>
                        {items.map(item => (
                            <tr key={item.id}>
                                <th scope="row">{item.id}</th>
                                <td>{item.name}</td>
                                <td>{item.genre}</td>
                                <td>{item.artist}</td>
                                <td>{item.language}</td>
                                <td>{item.coverImagePath}</td>
                                <td>{item.imageStorageName}</td>
                                <td><button className='edit'onClick={() => window.location.href="/adminpanel/music/edit/id=" + item.id}>Edit</button></td>
                                <td><button className='delete' onClick={() => window.location.href="/adminpanel/music/delete/id=" + item.id}>Delete</button></td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <div className='pagination'>
                    <button id="leftarrow" onClick={() => handlePagination(currentPage - 1, 10)}>&lt;</button>
                    <button id="rightarrow" onClick={() => handlePagination(currentPage + 1, 10)}>&gt;</button>
                </div>
            </div>
        </>
    )
}

export default AdminMusic
