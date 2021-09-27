<template>
    <div class="paging-bar">
        <div class="display-number paging-left">
            <div style="margin-left: 5px;">
                {{textResource['total']}}: <span
                class="bold-text">{{formatTotalRecord}}</span> {{textResource['record']}}
            </div>
        </div>

        <div class="paging-right">
            <BaseCombobox v-click-outside="hideBorderCombobox" @click.native="showBorderCombobox" ref="pageSize"
                directionDrop="dropup" class="cbo select-container filter-select" dataType="pageSize"
                @updateContent="updatePageSize" isDisabledInput="1" :isReadyData="isReady"
                :class="{'waiting': isChangeBorderCbo}" />
            <div class="paging paging-center">
                <BaseButton :isReadyData="isReady" @click="goToPreviousPage" class="large"
                    :classList="'btn-text large ' + (currentPage == 1 ? 'm-btn-disable' : '')" :disable="currentPage==1"
                    :btnText="pagingBarButtonContent['previous']" alt="" />
                <BaseButton :isReadyData="isReady" @click="goToPageNumber(pages[0])"
                    :classList="this.currentPage == pages[0] ? 'btn-text active-option' : 'btn-text'"
                    :btnText="pages[0].toString()" alt="" />
                <div v-if="(!toggleThreeDot || isBothDisplayThreeDots) && maxPageNumber >= 4"
                    @click="threeDotsFirstClick" class="three-dot three-dot-first">
                    ...
                </div>
                <BaseButton v-if="maxPageNumber >= 2" :isReadyData="isReady" @click="goToPageNumber(pages[1])"
                    :classList="this.currentPage == pages[1] ? 'btn-text active-option' : 'btn-text'"
                    :btnText="pages[1].toString()" alt="" />
                <BaseButton v-if="maxPageNumber >= 3" :isReadyData="isReady" @click="goToPageNumber(pages[2])"
                    :classList="this.currentPage == pages[2] ? 'btn-text active-option' : 'btn-text'"
                    :btnText="pages[2].toString()" alt="" />
                <BaseButton :isReadyData="isReady" v-if="this.isBothDisplayThreeDots" @click="goToPageNumber(pages[3])"
                    :classList="this.currentPage == pages[3] ? 'btn-text active-option' : 'btn-text'"
                    :btnText="pages[3].toString()" alt="" />
                <div v-if="(toggleThreeDot || isBothDisplayThreeDots) && maxPageNumber >= 4" @click="threeDotsLastClick"
                    class="three-dot three-dot-last">
                    ...
                </div>
                <BaseButton :isReadyData="isReady" @click="goToPageNumber(maxPageNumber)" v-if="maxPageNumber >= 4"
                    :classList="this.currentPage == maxPageNumber ? 'btn-text active-option' : 'btn-text'"
                    :btnText="maxPageNumber.toString()" alt="" />
                <BaseButton :isReadyData="isReady" @click="goToNextPage" class="large"
                    :btnText="pagingBarButtonContent['next']" alt=""
                    :classList="'btn-text large ' + (currentPage == maxPageNumber ? 'm-btn-disable' : '')"
                    :disable="currentPage==maxPageNumber" />
            </div>
        </div>
    </div>
</template>

<script>
    import BaseButton from "./BaseButton.vue"
    import BaseCombobox from "./BaseCombobox.vue"
    import Format from "../store/format.js"
    export default {
        props: ['maxPageNumber', 'pageSize', 'totalRecord', 'type', 'isReady'],
        data() {
            return {
                currentPage: 1,
                pages: [1, 2, 3, 4],
                pageSizeChange: 0,
                toggleThreeDot: true,
                isBothDisplayThreeDots: false,
                currentIndex: 0,
                isChangeBorderCbo: false
            }
        },
        mounted() {
            this.$refs.pageSize.setData(this.$store.state.data.pageSize.map(a => Object.assign({}, a)), true);
            this.$refs.pageSize.updateInput(this.$store.state.data.pageSize[0].content);
            this.currentPage = 1
            this.pages = [1, 2, 3, 4];
        },
        watch: {
            /**
             * Theo dõi sự thay đổi của trang hiện tại
             * Created By TBN (19/8/2021)
             */
            currentPage() {
                // Thay đổi index hiện tại của trang trong dãy pages
                let checkIndex = this.extractIndex(this.currentPage);
                this.currentIndex = (checkIndex != undefined ? checkIndex : this.currentIndex);
                // Hiện đồng thời 2 nút nếu khi ra không phải dãy 3 trang đầu và 3 trang cuối
                if (this.currentPage >= 4 && this.currentPage < this.maxPageNumber - 2) {
                    this.isBothDisplayThreeDots = true;
                } else {
                    this.isBothDisplayThreeDots = false;
                }
            }
        },
        components: {
            BaseButton,
            BaseCombobox,
        },
        methods: {
            /**
             * Ẩn border combobox chọn số bản ghi trên 1 trang
             * Created By TBN (28/7/2021)
             */
            hideBorderCombobox() {
                if (this.isChangeBorderCbo) {
                    this.isChangeBorderCbo = false;
                }
            },
            /**
             * Hiện border combobox chọn số bản ghi trên 1 trang
             * Created By TBN (28/7/2021)
             */
            showBorderCombobox() {
                this.isChangeBorderCbo = true;
            },
            /**
             * Sự kiện ghi bấm vào nút ... phía trước
             * Created By TBN (28/7/2021)
             */
            threeDotsFirstClick() {
                this.toggleThreeDot = true; // Chuyển việc hiện về nút ... sau
                this.isBothDisplayThreeDots = false; // Đưa giá trị hiện cùng về false
                this.pages = [1, 2, 3, 4]; // Thiết lập lại số trang
            },
            /**
             * Sự kiện bấm vào nut ... phía sau
             * Created By TBN (28/7/2021)
             */
            threeDotsLastClick() {
                this.isBothDisplayThreeDots = false; // Đưa giá trị hiện cùng về false
                this.toggleThreeDot = false; // Chuyển việc hiện về nút ... trước
                // Thiết lập lại số trang
                this.pages = [1, this.maxPageNumber - 2, this.maxPageNumber - 1, this.maxPageNumber];
            },
            /**
             * Sự kiện về trang trước
             * Created by TBN (21/7/2021)
             */
            async goToPreviousPage() {
                if (this.currentPage == this.pages[1]) {
                    // Nếu ở trang đầu tiên trong dãy 4 số paging bar
                    if (this.currentPage <= 4) {
                        this.pages = [1, 2, 3, 4] // Chuyển dãy 4 số về ban đầu
                        this.toggleThreeDot = true;
                    } else {
                        // Trừ dãy số paging bar thêm số lần tùy thuộc số trang lớn nhất
                        let subtratAmount = (this.maxPageNumber > 4 ? 3 : 4);
                        this.pages.forEach((element, index) => this.pages[index] = element -
                            subtratAmount)
                        this.pages[0] = 1;
                    }
                } else {
                    let shiftAmount = (this.maxPageNumber > 4 ? 3 : 4);
                    // Render lại paging nếu trang hiện tại nằm ngoài khoảng hiển thị của paging bar
                    if (this.currentPage - 1 + shiftAmount < this.maxPageNumber) {
                        switch (this.currentIndex) {
                            // Nếu là vị trí thứ 2 trang dãy
                            case 1:
                                if (this.currentPage - 1 < shiftAmount) {
                                    // Nếu trang sau khi bấm nhỏ hơn shiftAmount thì chuyển về dãy trang đầu tiên
                                    this.pages = [1, 2, 3, 4];
                                } else {
                                    // Trang đầu tiên luôn luôn là 1 và render lại trang phía sau
                                    // 1... [7] 8 9 ... 10 -> Previous -> 1 ... 4 5 [6] ... 10
                                    this.pages = [1, this.currentPage - 3, this.currentPage - 2, this
                                        .currentPage -
                                        1
                                    ];
                                }
                                break;
                            case 2:
                                // Render lại phòng trường hợp người dùng bấm nút ... rồi bấm back trang
                                this.pages = [1, this.currentPage - 1, this.currentPage, this.currentPage +
                                    1
                                ];
                                break;
                            case 3:
                                // Render lại phòng trường hợp người dùng bấm nút ... rồi bấm back trang
                                this.pages = [1, this.currentPage - 2, this.currentPage - 1, this
                                    .currentPage
                                ];
                                break;
                        }
                        this.toggleThreeDot = true; // Hiện ... phía sau
                    } else {
                        if (this.maxPageNumber >= 4) {
                            // Nếu thuộc dãy cuối cùng render lại trang đề phòng th người dùng bấm ...
                            if (this.currentIndex == 1) {
                                this.pages = [1, this.currentPage - 3, this.currentPage - 2, this.currentPage -
                                    1
                                ];
                                this.toggleThreeDot = true; // Hiện nút ... phía sau
                            } else {
                                this.pages = [1, this.maxPageNumber - 2, this.maxPageNumber - 1, this
                                    .maxPageNumber
                                ];
                                this.toggleThreeDot = false; // Hiện nút ... phía trước
                            }
                        }
                    }
                }
                this.currentPage-- // Thay đổi trang hiện tại
                this.$emit("updatePagination", this.currentPage - 1) // Emit sự kiện cập nhật trang dữ liệu
            },
            /**
             * Sự kiện click vào số trang tại paging bar
             * Created by TBN (21/7/2021)
             */
            async goToPageNumber(number) {
                // Kiểm tra xem có click chọn trang đầu hoặc trang cuối không
                // Nếu có thì sẽ render lại paging bar
                if (number == this.maxPageNumber) {
                    if (this.maxPageNumber >= 4) {
                        this.pages = [1, this.maxPageNumber - 2, this.maxPageNumber - 1, this.maxPageNumber];
                        this.toggleThreeDot = false;
                    }
                } else if (number == 1) {
                    this.pages = [1, 2, 3, 4];
                    this.toggleThreeDot = true;
                }
                // Chuyển đến trang được click
                this.$emit("updatePagination", number - 1)
                this.currentPage = number
            },
            /**
             * Chuyển đến trang tiếp theo
             * Created by TBN (21/7/2021)
             */
            async goToNextPage() {
                // Nếu có lỗi thêm dòng '&& this.maxPageNumber >= 4' vào if đầu tiên
                let shiftAmount = (this.maxPageNumber > 4 ? 3 : 4);
                if (this.currentPage + shiftAmount < this.maxPageNumber) {
                    // Nếu chưa phải dãy cuối render lại số trang phòng trường hợp người dùng bấm ...
                    switch (this.currentIndex) {
                        case 1:
                            this.pages = [1, this.currentPage, this.currentPage + 1, this.currentPage +
                                2
                            ]
                            break;
                        case 2:
                            this.pages = [1, this.currentPage - 1, this.currentPage, this.currentPage +
                                1
                            ]
                            if (this.currentPage < 4) {
                                this.pages = [1, this.currentPage + 1, this.currentPage + 2, this.currentPage +
                                    3
                                ]
                            }
                            break;
                        case 3:
                            this.pages = [1, this.currentPage + 1, this.currentPage + 2, this
                                .currentPage +
                                3
                            ]
                            break;
                    }
                    this.toggleThreeDot = true; //  Hiện nút ... phía sau
                } else {
                    if (this.maxPageNumber >= 4) {
                        // Nếu là trang cuối cùng thì render lại sô trang
                        this.pages = [1, this.maxPageNumber - 2, this.maxPageNumber - 1, this.maxPageNumber];
                        this.toggleThreeDot = false;
                    }
                }
                this.$emit("updatePagination", this.currentPage) // Emit sự kiện cập nhật trang dữ liệu
                this.currentPage += 1 // Thay đổi trang hiện tại
            },
            /**
             * Lấy index trang
             * Created By TBN(18/8/2021)
             */
            extractIndex(page) {
                let indexReturn = 0;
                this.pages.forEach((element, index) => {
                    if (element == page) {
                        indexReturn = index;
                        return;
                    }
                })
                return indexReturn;
            },
            /**
             * Cập nhật kích thước trang
             * Created By TBN (28/7/2021)
             */
            updatePageSize() {
                this.currentPage = 1
                this.pages = [1, 2, 3, 4];
                //this.pageSizeChange = sel.options[sel.selectedIndex].value;
                this.$emit('updatePageSize', this.$refs.pageSize.inputContent);
            }
        },
        computed: {
            textResource() {
                return this.$store.state.resource[this.$browserLocale]['common']['text'];
            },
            pagingBarButtonContent() {
                return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
                    'content'
                ]['pagingBarButton'];
            },
            formatTotalRecord(){
                return Format.formatMoney(this.totalRecord);
            }
        }
    }
</script>

<style scoped>
    .waiting {
        border-color: rgb(44, 160, 28) !important;
    }

    .three-dot {
        cursor: pointer;
        height: 20px;
        width: 20px;
        display: flex;
        justify-content: center;
        align-content: center;
    }

    .max-number select {
        margin-right: 5px;
        outline: none;
        border: none;
    }

    .max-number select,
    .max-number .page-size {
        display: inline-block;
    }

    .bold-text {
        font-family: "Roboto-Bold";
    }
</style>