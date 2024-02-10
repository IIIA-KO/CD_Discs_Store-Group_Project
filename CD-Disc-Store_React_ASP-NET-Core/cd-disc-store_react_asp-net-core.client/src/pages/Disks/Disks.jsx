import React, { useEffect, useState } from 'react'
import DiskSearch from '../../DiskSearch/DiskSearch';
import Pagination from '../../Pagination/Pagination';

const Disks = () => {
  const [discs, setDiscs] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const itemsPerPage = 20;

  useEffect(() => {
    const fetchDiscs = async () => {
      try {
        const response = await fetch(`https://localhost:7117/Discs/GetAll?skip=${(currentPage - 1) * itemsPerPage}&take=${itemsPerPage}`);
        const data = await response.json();
        console.log(data);
        setDiscs(data.items);
        setTotalPages(Math.ceil(data.countItems / itemsPerPage));
      }
      catch (error) {
        console.error(error);
      }
    };

    fetchDiscs();
  }, [currentPage]);

  const handlePageClick = (pageNumber) => {
    setCurrentPage(pageNumber);
  };


  return (
    <div>
      <DiskSearch
        discs={discs}
        currentPage={currentPage}
        itemsPerPage={itemsPerPage} />
      <Pagination
        currentPage={currentPage}
        totalPages={totalPages}
        onPageClick={handlePageClick} />
    </div>
  );
};

export default Disks;