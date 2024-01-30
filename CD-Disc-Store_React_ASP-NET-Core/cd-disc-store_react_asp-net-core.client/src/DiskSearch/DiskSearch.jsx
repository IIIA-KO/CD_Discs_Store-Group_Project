import React, { useState, useEffect } from 'react';
import "./DiskSearch.css"
import DiskCardList from '../DiskCardList/DiskCardList';
const DiskSearch = ({ disks }) => {
    const [search, setSearch] = useState('');
  
  const [filteredDisks, setFilteredDisks] = useState([]);

  useEffect(() => {
    const filtered = disks.filter((disk) => {
      const titleMatch = disk.name.toLowerCase().includes(search.toLowerCase());
      return titleMatch;
    });
    setFilteredDisks(filtered);
  }, [search, disks]);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
  }

  return (
    <div>
        <div className='search_bar'>
      <input type="text" placeholder='search Disk' value={search} onChange={handleSearchChange} />
      
      </div>
      {/* Отображение отфильтрованных результатов */}
      <ul>
        <DiskCardList data={filteredDisks} />
      </ul>
    </div>
  );
};

export default DiskSearch;
